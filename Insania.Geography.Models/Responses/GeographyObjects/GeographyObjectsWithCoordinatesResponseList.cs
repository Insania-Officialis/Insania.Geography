using Insania.Shared.Models.Responses.Base;

namespace Insania.Geography.Models.Responses.GeographyObjects;

/// <summary>
/// Модель элемента ответа списком географических объектов с координатами
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="List{GeographyObjectsWithCoordinatesResponseListItem}?" name="items">Список географических объектов с координатами</param>

public class GeographyObjectsWithCoordinatesResponseList(bool success, List<GeographyObjectsWithCoordinatesResponseListItem>? items = null) : BaseResponse(success)
{
    /// <summary>
    /// Список координат географических объектов
    /// </summary>
    public List<GeographyObjectsWithCoordinatesResponseListItem>? Items { get; set; } = items;
}