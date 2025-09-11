using NetTopologySuite.Geometries;

using Insania.Shared.Models.Responses.Base;

namespace Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

/// <summary>
/// Модель ответа списком координат географического объекта
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="long?" name="id">Идентификатор географического объекта</param>
/// <param cref="string?" name="name">Наименование географического объекта</param>
/// <param cref="Point?" name="center">Центр географического объекта</param>
/// <param cref="int?" name="zoom">Коэффициент масштаба отображения географического объекта</param>
/// <param cref="List{GeographyObjectCoordinatesResponseListItem}?" name="items">Список координат</param>

public class GeographyObjectCoordinatesResponseList(bool success, long? id = null, string? name = null, Point? center = null, int? zoom = null, List<GeographyObjectCoordinatesResponseListItem>? items = null) : BaseResponse(success, id)
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
    public List<GeographyObjectCoordinatesResponseListItem>? Items { get; set; } = items;
}