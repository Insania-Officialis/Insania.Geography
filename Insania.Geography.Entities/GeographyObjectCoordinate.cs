using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Insania.Shared.Entities;

namespace Insania.Geography.Entities;

/// <summary>
/// Модель географического объекта координаты географического объекта
/// </summary>
[Table("u_geography_objects_coordinates")]
[Comment("Координаты географических объектов")]
public class GeographyObjectCoordinate : EntityCoordinate
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели географического объекта координаты географического объекта
    /// </summary>
    public GeographyObjectCoordinate() : base()
    {
        GeographyObjectEntity = new();
    }

    /// <summary>
    /// Конструктор модели географического объекта координаты географического объекта без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Point" name="center">Координаты точки центра географического объекта</param>
    /// <param cref="double" name="area">Площадь географического объекта</param>
    /// <param cref="int" name="zoom">Коэффициент масштаба отображения географического объекта</param>
    /// <param cref="CoordinateGeography" name="coordinate">Координата</param>
    /// <param cref="GeographyObject" name="geographyObject">Географический объект</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public GeographyObjectCoordinate(string username, bool isSystem, Point center, double area, int zoom, CoordinateGeography coordinate, GeographyObject geographyObject, DateTime? dateDeleted = null) : base(username, isSystem, center, area, zoom, coordinate, dateDeleted)
    {
        GeographyObjectId = geographyObject.Id;
        GeographyObjectEntity = geographyObject;
    }

    /// <summary>
    /// Конструктор модели географического объекта координаты географического объекта с идентификатором
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Point" name="center">Координаты точки центра географического объекта</param>
    /// <param cref="double" name="area">Площадь географического объекта</param>
    /// <param cref="int" name="zoom">Коэффициент масштаба отображения географического объекта</param>
    /// <param cref="CoordinateGeography" name="coordinate">Координата</param>
    /// <param cref="GeographyObject" name="geographyObject">Географический объект</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public GeographyObjectCoordinate(long id, string username, bool isSystem, Point center, double area, int zoom, CoordinateGeography coordinate, GeographyObject geographyObject, DateTime? dateDeleted = null) : base(id, username, isSystem, center, area, zoom, coordinate, dateDeleted)
    {
        GeographyObjectId = geographyObject.Id;
        GeographyObjectEntity = geographyObject;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Идентификатор географического объекта
    /// </summary>
    [Column("geography_object_id")]
    [Comment("Идентификатор географического объекта")]
    public long GeographyObjectId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство географического объекта
    /// </summary>
    [ForeignKey("GeographyObjectId")]
    public GeographyObject? GeographyObjectEntity { get; private set; }
    #endregion

    #region Методы

    /// <summary>
    /// Метод записи координаты
    /// </summary>
    /// <param cref="GeographyObject" name="geographyObject">Географический объект</param>
    public void SetGeographyObject(GeographyObject geographyObject)
    {
        GeographyObjectId = geographyObject.Id;
        GeographyObjectEntity = geographyObject;
    }
    #endregion
}