namespace Insania.Geography.Models.Settings;

/// <summary>
/// Модель параметров инициализации данных
/// </summary>
public class InitializationDataSettings
{
    /// <summary>
    /// Признак инициализации структуры
    /// </summary>
    /// <remarks>
    /// Нужен для запуска миграций, при true не происходит инициализация данных
    /// </remarks>
    public bool? InitStructure { get; set; }

    /// <summary>
    /// Путь к файлам скриптов
    /// </summary>
    public string? ScriptsPath { get; set; }

    /// <summary>
    /// Включение в инициализацию таблиц
    /// </summary>
    public InitializationDataSettingsIncludeTables? Tables { get; set; }

    /// <summary>
    /// Включение в инициализацию баз данных
    /// </summary>
    public InitializationDataSettingsIncludeDatabases? Databases { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию таблиц
/// </summary>
public class InitializationDataSettingsIncludeTables
{
    /// <summary>
    /// Типы географических объектов
    /// </summary>
    public bool? GeographyObjectsTypes { get; set; }

    /// <summary>
    /// Географические объекты
    /// </summary>
    public bool? GeographyObjects { get; set; }

    /// <summary>
    /// Типы координат
    /// </summary>
    public bool? CoordinatesTypes { get; set; }

    /// <summary>
    /// Координаты
    /// </summary>
    public bool? Coordinates { get; set; }

    /// <summary>
    /// Координаты географических объектов
    /// </summary>
    public bool? GeographyObjectsCoordinates { get; set; }

    /// <summary>
    /// Рельефы
    /// </summary>
    public bool? Reliefs { get; set; }
}

/// <summary>
/// Модель параметра включения в инициализацию баз данных
/// </summary>
public class InitializationDataSettingsIncludeDatabases
{
    /// <summary>
    /// География
    /// </summary>
    public bool? Geography { get; set; }

    /// <summary>
    /// Логи сервиса географии
    /// </summary>
    public bool? LogsApiGeography { get; set; }
}