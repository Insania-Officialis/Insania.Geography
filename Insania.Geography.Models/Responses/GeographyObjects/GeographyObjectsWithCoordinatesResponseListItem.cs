using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

namespace Insania.Geography.Models.Responses.GeographyObjects;

/// <summary>
/// Модель ответа списком географических объектов с координатами
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="long?" name="id">Идентификатор географического объекта</param>
/// <param cref="string?" name="name">Наименование географического объекта</param>
/// <param cref="double?[]?" name="center">Центр географического объекта</param>
/// <param cref="int?" name="zoom">Коэффициент масштаба отображения географического объекта</param>
/// <param cref="List{GeographyObjectCoordinatesResponseListItem}?" name="coordinates">Список координат</param>

public class GeographyObjectsWithCoordinatesResponseListItem(long? id = null, string? name = null, double?[]? center = null, int? zoom = null, List<GeographyObjectCoordinatesResponseListItem>? coordinates = null)
{
    /// <summary>
    /// Идентификатор географического объекта
    /// </summary>
    public long? Id { get; set; } = id;

    /// <summary>
    /// Наименование географического объекта
    /// </summary>
    public string? Name { get; set; } = name;

    /// <summary>
    /// Центр географического объекта
    /// </summary>
    public double?[]? Center { get; set; } = center;

    /// <summary>
    /// Коэффициент масштаба отображения географического объекта
    /// </summary>
    public int? Zoom { get; set; } = zoom;

    /// <summary>
    /// Список координат
    /// </summary>
    public List<GeographyObjectCoordinatesResponseListItem>? Coordinates { get; set; } = coordinates;
}