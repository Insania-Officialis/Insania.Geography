using Microsoft.EntityFrameworkCore;

using Insania.Geography.Entities;

namespace Insania.Geography.Database.Contexts;

/// <summary>
/// Контекст бд логов сервиса географии
/// </summary>
public class LogsApiGeographyContext : DbContext
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор контекста бд логов сервиса географии
    /// </summary>
    public LogsApiGeographyContext() : base()
    {

    }

    /// <summary>
    /// Конструктор контекста бд логов сервиса географии с опциями
    /// </summary>
    /// <param cref="DbContextOptions{LogsApiGeographyContext}" name="options">Параметры</param>
    public LogsApiGeographyContext(DbContextOptions<LogsApiGeographyContext> options) : base(options)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Логи
    /// </summary>
    public virtual DbSet<LogApiGeography> Logs { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод при создании моделей
    /// </summary>
    /// <param cref="ModelBuilder" name="modelBuilder">Конструктор моделей</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Установка схемы бд
        modelBuilder.HasDefaultSchema("insania_logs_api_geography");
        
        //Добавление gin-индекса на поле с входными данными логов
        modelBuilder.Entity<LogApiGeography>().HasIndex(x => x.DataIn).HasMethod("gin");

        //Добавление gin-индекса на поле с выходными данными логов
        modelBuilder.Entity<LogApiGeography>().HasIndex(x => x.DataOut).HasMethod("gin");
    }
    #endregion
}