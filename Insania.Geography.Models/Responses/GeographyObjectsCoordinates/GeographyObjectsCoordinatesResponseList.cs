using NetTopologySuite.Geometries;

using Insania.Shared.Models.Responses.Base;

namespace Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

/// <summary>
/// Модель ответа списком координат географических объектов
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="long?" name="id">Идентификатор географического объекта</param>
/// <param cref="string?" name="name">Наименование географического объекта</param>
/// <param cref="Point?" name="center">Центр географического объекта</param>
/// <param cref="int?" name="zoom">Коэффициент масштаба отображения географического объекта</param>
/// <param cref="List{GeographyObjectsCoordinatesResponseListItem}?" name="items">Список координат</param>

public class GeographyObjectsCoordinatesResponseList(bool success, long? id = null, string? name = null, Point? center = null, int? zoom = null, List<GeographyObjectsCoordinatesResponseListItem>? items = null) : BaseResponse(success, id)
{
    /// <summary>
    /// Наименование географического объекта
    /// </summary>
    public string? Name { get; set; } = name;

    /// <summary>
    /// Центр географического объекта
    /// </summary>
    public Point? Center { get; set; } = center;

    /// <summary>
    /// Коэффициент масштаба отображения географического объекта
    /// </summary>
    public int? Zoom { get; set; } = zoom;

    /// <summary>
    /// Список координат
    /// </summary>
    public List<GeographyObjectsCoordinatesResponseListItem>? Items { get; set; } = items;
}