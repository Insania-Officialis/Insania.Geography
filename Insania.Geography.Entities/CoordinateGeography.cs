using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using NetTopologySuite.Geometries;

using Coordinate = Insania.Shared.Entities.Coordinate;

namespace Insania.Geography.Entities;

/// <summary>
/// Модель сущности координаты географии
/// </summary>
[Table("r_coordinates")]
[Comment("Координаты географии")]
public class CoordinateGeography : Coordinate
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности координаты географии
    /// </summary>
    public CoordinateGeography() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности координаты географии без идентификатора
    /// </summary>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Polygon" name="polygon">Полигон (массив координат)</param>
    /// <param cref="CoordinateTypeGeography" name="type">Тип</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinateGeography(string username, bool isSystem, Polygon polygon, CoordinateTypeGeography type, DateTime? dateDeleted = null) : base(username, isSystem, polygon, type, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности координаты географии с идентификатором
    /// </summary>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="bool" name="isSystem">Признак системной записи</param>
    /// <param cref="Polygon" name="polygon">Полигон (массив координат)</param>
    /// <param cref="CoordinateTypeGeography" name="type">Тип</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinateGeography(long id, string username, bool isSystem, Polygon polygon, CoordinateTypeGeography type, DateTime? dateDeleted = null) : base(id, username, isSystem, polygon, type, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности координаты географии с сущностью
    /// </summary>
    /// <param cref="Coordinate" name="entity">Базовая сущность</param>
    public CoordinateGeography(Coordinate entity) : base(entity.Id, entity.UsernameCreate, entity.IsSystem, entity.PolygonEntity, entity.TypeEntity!, entity.DateDeleted)
    {

    }
    #endregion
}