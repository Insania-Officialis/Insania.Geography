using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Shared.Services;

using Insania.Geography.BusinessLogic;
using Insania.Geography.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Models.Mapper;
using Insania.Geography.Models.Settings;

namespace Insania.Geography.Tests.Base;

/// <summary>
/// Базовый класс тестирования
/// </summary>
public abstract class BaseTest
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор базового класса тестирования
    /// </summary>
    public BaseTest()
    {
        //Создание коллекции сервисов
        IServiceCollection services = new ServiceCollection();

        //Создание коллекции ключей конфигурации
        Dictionary<string, string> configurationKeys = new()
        {
           {"LoggingOptions:FilePath", DetermineLogPath()},
           {"InitializationDataSettings:ScriptsPath", DetermineScriptsPath()},
           {"InitializationDataSettings:InitStructure", "false"},
           {"InitializationDataSettings:Tables:GeographyObjectsTypes", "true"},
           {"InitializationDataSettings:Tables:GeographyObjects", "true"},
           {"InitializationDataSettings:Tables:CoordinatesTypes", "true"},
           {"InitializationDataSettings:Tables:Coordinates", "true"},
           {"InitializationDataSettings:Tables:GeographyObjectsCoordinates", "true"},
           {"InitializationDataSettings:Tables:Reliefs", "true"},
           {"InitializationDataSettings:Tables:ReliefsCoordinates", "true"},
           {"TokenSettings:Issuer", "Geography.Test"},
           {"TokenSettings:Audience", "Geography.Test"},
           {"TokenSettings:Key", "This key is generated for tests in the user zone"},
           {"TokenSettings:Expires", "7"},
        };

        //Создание экземпляра конфигурации в памяти
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(configurationKeys!).Build();

        //Установка игнорирования типов даты и времени
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //Внедрение зависимостей сервисов
        services.AddSingleton(_ => configuration); //конфигурация
        services.AddScoped<ITransliterationSL, TransliterationSL>(); //сервис транслитерации
        services.AddScoped<IPolygonParserSL, PolygonParserSL>(); //сервис преобразования полигонов
        services.AddScoped<IInitializationDAO, InitializationDAO>(); //сервис инициализации данных в бд географии
        services.AddGeographyBL(); //сервисы работы с бизнес-логикой в зоне географии

        //Добавление контекстов бд в коллекцию сервисов
        services.AddDbContext<GeographyContext>(options => options.UseInMemoryDatabase(databaseName: "insania_geography").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд географии
        services.AddDbContext<LogsApiGeographyContext>(options => options.UseInMemoryDatabase(databaseName: "insania_logs_api_geography").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))); //бд логов сервиса географии

        //Добавление параметров логирования
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        //Добавление параметров преобразования моделей
        services.AddAutoMapper(cfg => cfg.AddProfile<GeographyMappingProfile>());

        //Добавление параметров инициализации данных
        IConfigurationSection? initializationDataSettings = configuration.GetSection("InitializationDataSettings");
        services.Configure<InitializationDataSettings>(initializationDataSettings);

        //Создание поставщика сервисов
        ServiceProvider = services.BuildServiceProvider();

        //Выполнение инициализации данных
        IInitializationDAO initialization = ServiceProvider.GetRequiredService<IInitializationDAO>();
        initialization.Initialize().Wait();
    }
    #endregion

    #region Поля
    /// <summary>
    /// Поставщик сервисов
    /// </summary>
    protected IServiceProvider ServiceProvider { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод определения пути для логов
    /// </summary>
    /// <returns cref="string">Путь для сохранения логов</returns>
    private static string DetermineLogPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        //Возврат нужного пути
        if (isRunningInDocker) return "/logs/log.txt";
        else return "G:\\Program\\Insania\\Logs\\Geography.Tests\\log.txt";
    }

    /// <summary>
    /// Метод определения пути для скриптов
    /// </summary>
    /// <returns cref="string">Путь к скриптам</returns>
    private static string DetermineScriptsPath()
    {
        //Проверка запуска в докере
        bool isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" || File.Exists("/.dockerenv");

        if (isRunningInDocker) return "/src/Insania.Geography.Database/Scripts";
        else return "G:\\Program\\Insania\\Insania.Geography\\Insania.Geography.Database\\Scripts";
    }
    #endregion
}