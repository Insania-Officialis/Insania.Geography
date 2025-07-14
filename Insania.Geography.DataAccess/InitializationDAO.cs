using System.Text.Json;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetTopologySuite.Geometries;
using Npgsql;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;

using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;
using Insania.Geography.Models.Settings;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.Shared.Messages.InformationMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Сервис инициализации данных в бд географии
/// </summary>
/// <param cref="ILogger{InitializationDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="geographyContext">Контекст базы данных географии</param>
/// <param cref="LogsApiGeographyContext" name="logsApiGeographyContext">Контекст базы данных логов сервиса географии</param>
/// <param cref="IOptions{InitializationDataSettings}" name="settings">Параметры инициализации данных</param>
/// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
/// <param cref="IPolygonParserSL" name="polygonParser">Сервис преобразования полигона</param>
/// <param cref="IConfiguration" name="configuration">Конфигурация приложения</param>
public class InitializationDAO(ILogger<InitializationDAO> logger, GeographyContext geographyContext, LogsApiGeographyContext logsApiGeographyContext, IOptions<InitializationDataSettings> settings, ITransliterationSL transliteration, IPolygonParserSL polygonParser, IConfiguration configuration) : IInitializationDAO
{
    #region Поля
    private readonly string _username = "initializer";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<InitializationDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _geographyContext = geographyContext;

    /// <summary>
    /// Контекст базы данных логов сервиса географии
    /// </summary>
    private readonly LogsApiGeographyContext _logsApiGeographyContext = logsApiGeographyContext;

    /// <summary>
    /// Параметры инициализации данных
    /// </summary>
    private readonly IOptions<InitializationDataSettings> _settings = settings;

    /// <summary>
    /// Сервис транслитерации
    /// </summary>
    private readonly ITransliterationSL _transliteration = transliteration;

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private readonly IPolygonParserSL _polygonParser = polygonParser;

    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    private readonly IConfiguration _configuration = configuration;
    #endregion

    #region Методы
    /// <summary>
    /// Метод инициализации данных
    /// </summary>
    /// <exception cref="Exception">Исключение</exception>
    public async Task Initialize()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredInitializeMethod);

            //Инициализация структуры
            if (_settings.Value.InitStructure == true)
            {
                //Логгирование
                _logger.LogInformation("{text}", InformationMessages.InitializationStructure);

                //Инициализация баз данных в зависимости от параметров
                if (_settings.Value.Databases?.Geography == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("GeographySever") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_geography_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("GeographyEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_geography_\d+\.sql$";
                    string patternExtension = @"^extension_geography_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes, patternExtension);
                }
                if (_settings.Value.Databases?.LogsApiGeography == true)
                {
                    //Формирование параметров
                    string connectionServer = _configuration.GetConnectionString("LogsApiGeographyServer") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternDatabases = @"^databases_logs_api_geography_\d+\.sql$";
                    string connectionDatabase = _configuration.GetConnectionString("LogsApiGeographyEmpty") ?? throw new Exception(ErrorMessagesShared.EmptyConnectionString);
                    string patternSchemes = @"^schemes_logs_api_geography_\d+\.sql$";

                    //Создание базы данных
                    await CreateDatabase(connectionServer, patternDatabases, connectionDatabase, patternSchemes);
                }

                //Выход
                return;
            }

            //Накат миграций
            if (_geographyContext.Database.IsRelational()) await _geographyContext.Database.MigrateAsync();
            if (_logsApiGeographyContext.Database.IsRelational()) await _logsApiGeographyContext.Database.MigrateAsync();

            //Проверки
            if (string.IsNullOrWhiteSpace(_settings.Value.ScriptsPath)) throw new Exception(ErrorMessagesShared.EmptyScriptsPath);

            //Инициализация данных в зависимости от параметров
            if (_settings.Value.Tables?.GeographyObjectsTypes == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _geographyContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<GeographyObjectType> entities =
                    [
                        new(_transliteration, 1, _username, "Удалённый", DateTime.UtcNow),
                        new(_transliteration, 2, _username, "Океан", null),
                        new(_transliteration, 3, _username, "Море", null),
                        new(_transliteration, 4, _username, "Материк", null),
                        new(_transliteration, 5, _username, "Архипелаг", null),
                        new(_transliteration, 6, _username, "Остров", null),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_geographyContext.GeographyObjectsTypes.Any(x => x.Id == entity.Id)) await _geographyContext.GeographyObjectsTypes.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _geographyContext.SaveChangesAsync();

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_geography_objects_types_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _geographyContext);
                    }

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.GeographyObjects == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _geographyContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "Асфалия", "4", "", ""],
                        ["2", "Эмбрия", "4", "", ""],
                        ["3", "Арборибус", "4", "", ""],
                        ["4", "Дитика", "4", "", ""],
                        ["5", "Акраия", "4", "", ""],
                        ["6", "Зимний архипелаг", "5", "", ""],
                        ["7", "Забытые острова", "5", "", ""],
                        ["8", "Морозный клык", "6", "", ""],
                        ["9", "Тёмные крылья", "6", "", ""],
                        ["10", "Северный щит", "6", "", ""],
                        ["11", "Челюсти", "6", "", ""],
                        ["12", "Клинок", "6", "", ""],
                        ["13", "Западный щит", "6", "", ""],
                        ["14", "Восточный щит", "6", "", ""],
                        ["15", "Южный щит", "6", "", ""],
                        ["16", "Железный", "6", "", ""],
                        ["17", "Рыбий", "6", "", ""],
                        ["18", "Дальний", "6", "", ""],
                        ["19", "Мраа", "6", "", ""],
                        ["20", "Буранов", "6", "", ""],
                        ["21", "Старший странник", "6", "", ""],
                        ["22", "Младший странник", "6", "", ""],
                        ["23", "Высокий", "6", "", ""],
                        ["24", "Чёрный", "6", "", ""],
                        ["25", "Ягодный", "6", "", ""],
                        ["26", "Мамонтова колыбель", "6", "", ""],
                        ["27", "Бивень мамонта", "6", "", ""],
                        ["28", "Пастбище мамонтов", "6", "", ""],
                        ["29", "Дамара", "6", "", ""],
                        ["30", "Кедровый", "6", "", ""],
                        ["31", "Вороний глаз", "6", "", ""],
                        ["32", "Мятный", "6", "", ""],
                        ["33", "Толстая сестра", "6", "", ""],
                        ["34", "Худощавая сестра", "6", "", ""],
                        ["35", "Кривая сестра", "6", "", ""],
                        ["36", "Стройная сестра", "6", "", ""],
                        ["37", "Проклятый", "6", "", ""],
                        ["38", "Каменный шлем", "6", "", ""],
                        ["39", "Глаз Моря", "6", "", ""],
                        ["40", "Королевский", "6", "", ""],
                        ["41", "Летний", "6", "", ""],
                        ["42", "Стальной", "6", "", ""],
                        ["43", "Стального дракона", "6", "", ""],
                        ["10000", "Удалённая", "1", "", DateTime.UtcNow.ToString()],
                        ["10001", "Тестовая", "4", "", ""],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_geographyContext.GeographyObjects.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            GeographyObjectType type = await _geographyContext.GeographyObjectsTypes.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObjectType);
                            GeographyObject? parent = !string.IsNullOrWhiteSpace(key[3])
                                ? (await _geographyContext.GeographyObjects.FirstOrDefaultAsync(x => x.Id == long.Parse(key[3])) ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject))
                                : null;

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[4])) dateDeleted = DateTime.Parse(key[4]);
                            GeographyObject entity = new(_transliteration, long.Parse(key[0]), _username, key[1], type, parent, dateDeleted);

                            //Добавление сущности в бд
                            await _geographyContext.GeographyObjects.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_geography_objects_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _geographyContext);
                    }

                    //Сохранение изменений в бд
                    await _geographyContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.CoordinatesTypes == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _geographyContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции сущностей
                    List<CoordinateTypeGeography> entities =
                    [
                        new(_transliteration, 1, _username, "Удалённый", "", "", DateTime.UtcNow),
                        new(_transliteration, 2, _username, "Океаны", "#E0E1DC", "#000000", null),
                        new(_transliteration, 3, _username, "Моря", "#E0E1DC", "#000000", null),
                        new(_transliteration, 4, _username, "Материки", "#FFFFFF", "#000000", null),
                    ];

                    //Проход по коллекции сущностей
                    foreach (var entity in entities)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_geographyContext.CoordinatesTypes.Any(x => x.Id == entity.Id)) await _geographyContext.CoordinatesTypes.AddAsync(entity);
                    }

                    //Сохранение изменений в бд
                    await _geographyContext.SaveChangesAsync();

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_coordinates_types_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _geographyContext);
                    }

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.Coordinates == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _geographyContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "[[[0,0],[0,5],[5,0],[0,0]]]", "1", DateTime.UtcNow.ToString()],
                        ["2", "[[[0,0],[0,5],[5,0],[0,0]]]", "4", ""],
                        ["3", "[[[0,0],[0,5],[5,0],[0,0]]]", "4", ""],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_geographyContext.Coordinates.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            CoordinateTypeGeography type = await _geographyContext.CoordinatesTypes.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesGeography.NotFoundCoordinateType);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            double[][][] coordinates = JsonSerializer.Deserialize<double[][][]>(key[1]) ?? throw new Exception(ErrorMessagesShared.EmptyCoordinates);
                            Polygon polygon = _polygonParser.FromDoubleArrayToPolygon(coordinates) ?? throw new Exception(ErrorMessagesShared.IncorrectCoordinates);
                            CoordinateGeography entity = new(long.Parse(key[0]), _username, true, polygon, type, dateDeleted);

                            //Добавление сущности в бд
                            await _geographyContext.Coordinates.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_coordinates_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _geographyContext);
                    }

                    //Сохранение изменений в бд
                    await _geographyContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
            if (_settings.Value.Tables?.GeographyObjectsCoordinates == true)
            {
                //Открытие транзакции
                IDbContextTransaction transaction = _geographyContext.Database.BeginTransaction();

                try
                {
                    //Создание коллекции ключей
                    string[][] keys =
                    [
                        ["1", "1", "1", DateTime.UtcNow.ToString()],
                        ["2", "2", "1", ""],
                        ["3", "3", "10001", DateTime.UtcNow.ToString()],
                    ];

                    //Проход по коллекции ключей
                    foreach (var key in keys)
                    {
                        //Добавление сущности в бд при её отсутствии
                        if (!_geographyContext.GeographyObjectsCoordinates.Any(x => x.Id == long.Parse(key[0])))
                        {
                            //Получение сущностей
                            CoordinateGeography coordinate = await _geographyContext.Coordinates.FirstOrDefaultAsync(x => x.Id == long.Parse(key[1])) ?? throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);
                            GeographyObject geographyObject = await _geographyContext.GeographyObjects.FirstOrDefaultAsync(x => x.Id == long.Parse(key[2])) ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

                            //Создание сущности
                            DateTime? dateDeleted = null;
                            if (!string.IsNullOrWhiteSpace(key[3])) dateDeleted = DateTime.Parse(key[3]);
                            Point center = coordinate.PolygonEntity.InteriorPoint;
                            double area = coordinate.PolygonEntity.Area;
                            int zoom = 3;
                            GeographyObjectCoordinate entity = new(long.Parse(key[0]), _username, true, center, area, zoom, coordinate, geographyObject, dateDeleted);

                            //Добавление сущности в бд
                            await _geographyContext.GeographyObjectsCoordinates.AddAsync(entity);
                        }
                    }

                    //Создание шаблона файла скриптов
                    string pattern = @"^t_geography_objects_coordinates_\d+.sql";

                    //Проходим по всем скриптам
                    foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), pattern)))
                    {
                        //Выполняем скрипт
                        await ExecuteScript(file, _geographyContext);
                    }

                    //Сохранение изменений в бд
                    await _geographyContext.SaveChangesAsync();

                    //Фиксация транзакции
                    transaction.Commit();
                }
                catch (Exception)
                {
                    //Откат транзакции
                    transaction.Rollback();

                    //Проброс исключения
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Метод создание базы данных
    /// </summary>
    /// <param cref="string" name="connectionServer">Строка подключения к серверу</param>
    /// <param cref="string" name="patternDatabases">Шаблон файлов создания базы данных</param>
    /// <param cref="string" name="connectionDatabase">Строка подключения к базе данных</param>
    /// <param cref="string" name="patternSchemes">Шаблон файлов создания схемы</param>
    /// <param cref="string?" name="patternExtension">Шаблон файлов создания расширений</param>
    /// <returns></returns>
    private async Task CreateDatabase(string connectionServer, string patternDatabases, string connectionDatabase, string patternSchemes, string? patternExtension = null)
    {
        //Проход по всем скриптам в директории и создание баз данных
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternDatabases)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionServer);
        }

        //Проход по всем скриптам в директории и создание схем
        foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternSchemes)))
        {
            //Выполнение скрипта
            await ExecuteScript(file, connectionDatabase);
        }

        //Проход по всем скриптам в директории и создание расширений
        if (!string.IsNullOrWhiteSpace(patternExtension))
        {
            foreach (var file in Directory.GetFiles(_settings.Value.ScriptsPath!).Where(x => Regex.IsMatch(Path.GetFileName(x), patternExtension)))
            {
                //Выполнение скрипта
                await ExecuteScript(file, connectionDatabase);
            }
        }
    }

    /// <summary>
    /// Метод выполнения скрипта со строкой подключения
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="string" name="connectionString">Строка подключения</param>
    private async Task ExecuteScript(string filePath, string connectionString)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Создание соединения к бд
            using NpgsqlConnection connection = new(connectionString);

            //Открытие соединения
            connection.Open();

            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Создание sql-запроса
            using NpgsqlCommand command = new(sql, connection);

            //Выполнение команды
            await command.ExecuteNonQueryAsync();

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }

    /// <summary>
    /// Метод выполнения скрипта с контекстом
    /// </summary>
    /// <param cref="string" name="filePath">Путь к скрипту</param>
    /// <param cref="DbContext" name="context">Контекст базы данных</param>
    private async Task ExecuteScript(string filePath, DbContext context)
    {
        //Логгирование
        _logger.LogInformation("{text} {params}", InformationMessages.ExecuteScript, filePath);

        try
        {
            //Считывание запроса
            string sql = File.ReadAllText(filePath);

            //Выполнение sql-команды
            await context.Database.ExecuteSqlRawAsync(sql);

            //Логгирование
            _logger.LogInformation("{text} {params}", InformationMessages.ExecutedScript, filePath);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {params} из-за ошибки {ex}", ErrorMessagesShared.NotExecutedScript, filePath, ex);
        }
    }
    #endregion
}