using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Serilog;

using Insania.Shared.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Shared.Messages;
using Insania.Shared.Services;

using Insania.Geography.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Models.Settings;

//Запуск хоста
CreateHostBuilder(args).Build().Run();

//Построение хоста
static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).ConfigureServices(async (hostContext, services) =>
    {
        //Добавление конфигурации в проект
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
#if DEBUG
            .AddJsonFile("appsettings.Development.json", true, false)
#else
            .AddJsonFile("appsettings.Production.json", true, false)
#endif
            .Build();

        //Установка игнорирования типов даты и времени
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //Внедрение зависимостей сервисов
        services.AddSingleton(configuration); //конфигурация
        services.AddScoped<ITransliterationSL, TransliterationSL>(); //сервис транслитерации
        services.AddScoped<IPolygonParserSL, PolygonParserSL>(); //сервис преобразования полигонов
        services.AddScoped<IInitializationDAO, InitializationDAO>(); //сервис инициализации данных в бд географии

        //Добавление контекстов бд в коллекцию сервисов
        services.AddDbContext<GeographyContext>(options =>
        {
            string connectionString = configuration.GetConnectionString("Geography") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
            options.UseNpgsql(
                connectionString,
                x => x.UseNetTopologySuite()
            );
        }); //бд географии
        services.AddDbContext<LogsApiGeographyContext>(options =>
        {
            string connectionString = configuration.GetConnectionString("LogsApiGeography") ?? throw new Exception(ErrorMessages.EmptyConnectionString);
            options.UseNpgsql(connectionString);
        }); //бд логов сервиса географии

        //Добавление параметров логирования
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(path: configuration["LoggingOptions:FilePath"]!, rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, dispose: true));

        //Добавление параметров инициализации данных
        IConfigurationSection? settings = configuration.GetSection("InitializationDataSettings");
        services.Configure<InitializationDataSettings>(settings);

        //Создание поставщика сервисов
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        //Инициализация данных, если не установлен признак инициализации структуры
        IOptions<InitializationDataSettings> initializeDataSettings = serviceProvider.GetRequiredService<IOptions<InitializationDataSettings>>();
        await serviceProvider.GetRequiredService<IInitializationDAO>().Initialize();
    }
);