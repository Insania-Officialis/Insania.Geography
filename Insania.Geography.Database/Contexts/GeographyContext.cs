using Microsoft.EntityFrameworkCore;

using Insania.Shared.Entities;

using Insania.Geography.Entities;

namespace Insania.Geography.Database.Contexts;

/// <summary>
/// Контекст бд географии
/// </summary>
public class GeographyContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд географии
    /// </summary>
    public GeographyContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд географии с опциями
    /// </summary>
    /// <param cref="DbContextOptions{GeographyContext}" name="options">Параметры</param>
    public GeographyContext(DbContextOptions<GeographyContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Типы географических объектов
    /// </summary>
    public virtual DbSet<GeographyObjectType> GeographyObjectsTypes { get; set; }

    /// <summary>
    /// Географические объекты
    /// </summary>
    public virtual DbSet<GeographyObject> GeographyObjects { get; set; }

    /// <summary>
    /// Типы координат
    /// </summary>
    public virtual DbSet<CoordinateTypeGeography> CoordinatesTypes { get; set; }

    /// <summary>
    /// Координаты
    /// </summary>
    public virtual DbSet<CoordinateGeography> Coordinates { get; set; }

    /// <summary>
    /// Координаты географических объектов
    /// </summary>
    public virtual DbSet<GeographyObjectCoordinate> GeographyObjectsCoordinates { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_geography");

        //Проверка наличия расширения
        modelBuilder.HasPostgresExtension("postgis");

        //Смена базовой модели типа координаты
        modelBuilder.Ignore<CoordinateType>();
        modelBuilder.Entity<CoordinateTypeGeography>();

        //Смена базовой модели координаты
        modelBuilder.Ignore<Coordinate>();
        modelBuilder.Entity<CoordinateGeography>();

        //Создание ограничения уникальности на псевдоним типа географического объекта
        modelBuilder.Entity<GeographyObjectType>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним наименования географического объекта
        modelBuilder.Entity<GeographyObject>().HasAlternateKey(x => x.Alias);

        //Создание ограничения уникальности на псевдоним типа координаты
        modelBuilder.Entity<CoordinateTypeGeography>().HasAlternateKey(x => x.Alias);

        //Добавление gin-индекса на поле с координатами
        modelBuilder.Entity<CoordinateGeography>().HasIndex(x => x.PolygonEntity).HasMethod("gist");

        //Создание ограничения уникальности на координату географического объекта
        modelBuilder.Entity<GeographyObjectCoordinate>().HasAlternateKey(x => new { x.CoordinateId, x.GeographyObjectId });
    }
    #endregion
}