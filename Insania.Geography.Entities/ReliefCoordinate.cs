using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Insania.Shared.Entities;

namespace Insania.Geography.Entities;

/// <summary>
/// Модель координаты рельефа
/// </summary>
[Table("u_reliefs_coordinates")]
[Comment("Координаты рельефов")]
public class ReliefCoordinate : EntityCoordinate
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели координаты рельефа
    /// </summary>
    public ReliefCoordinate() : base()
    {
        ReliefEntity = new();
    }

    /// <summary>
    /// Конструктор модели координаты рельефа без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Point" name="center">Координаты точки центра рельефа</param>
    /// <param cref="double" name="area">Площадь рельефа</param>
    /// <param cref="int" name="zoom">Коэффициент масштаба отображения рельефа</param>
    /// <param cref="CoordinateGeography" name="coordinate">Координата</param>
    /// <param cref="Relief" name="relief">Рельеф</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ReliefCoordinate(string username, bool isSystem, Point center, double area, int zoom, CoordinateGeography coordinate, Relief relief, DateTime? dateDeleted = null) : base(username, isSystem, center, area, zoom, coordinate, dateDeleted)
    {
        ReliefId = relief.Id;
        ReliefEntity = relief;
    }

    /// <summary>
    /// Конструктор модели координаты рельефа с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Point" name="center">Координаты точки центра рельефа</param>
    /// <param cref="double" name="area">Площадь рельефа</param>
    /// <param cref="int" name="zoom">Коэффициент масштаба отображения рельефа</param>
    /// <param cref="CoordinateGeography" name="coordinate">Координата</param>
    /// <param cref="Relief" name="relief">Рельеф</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public ReliefCoordinate(long id, string username, bool isSystem, Point center, double area, int zoom, CoordinateGeography coordinate, Relief relief, DateTime? dateDeleted = null) : base(id, username, isSystem, center, area, zoom, coordinate, dateDeleted)
    {
        ReliefId = relief.Id;
        ReliefEntity = relief;
    }

    /// <summary>
    /// Конструктор модели координаты рельефа с идентификатором
    /// </summary>
    /// <param cref="ReliefCoordinate" name="entity">Базовая сущность</param>
    public ReliefCoordinate(ReliefCoordinate entity) : this(entity.Id, entity.UsernameCreate, entity.IsSystem, entity.Center, entity.Area, entity.Zoom, (entity.CoordinateEntity as CoordinateGeography)!, entity.ReliefEntity!, entity.DateDeleted)
    {

    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор рельефа
    /// </summary>
    [Column("relief_id")]
    [Comment("Идентификатор рельефа")]
    public long ReliefId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство рельефа
    /// </summary>
    [ForeignKey("ReliefId")]
    public Relief? ReliefEntity { get; private set; }
    #endregion

    #region Методы

    /// <summary>
    /// Метод записи координаты
    /// </summary>
    /// <param cref="Relief" name="relief">Рельеф</param>
    public void SetRelief(Relief relief)
    {
        ReliefId = relief.Id;
        ReliefEntity = relief;
    }
    #endregion
}