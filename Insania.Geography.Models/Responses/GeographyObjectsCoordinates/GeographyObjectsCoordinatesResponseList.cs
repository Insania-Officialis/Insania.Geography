using Insania.Shared.Models.Responses.Base;

namespace Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

/// <summary>
/// Модель ответа списком координат географических объектов
/// </summary>
/// <param cref="bool" name="success">Признак успешности</param>
/// <param cref="List{GeographyObjectCoordinatesResponseListItem}?" name="items">Список координат географических объектов</param>

public class GeographyObjectsCoordinatesResponseList(bool success, List<GeographyObjectCoordinatesResponseList>? items = null) : BaseResponse(success)
{
    /// <summary>
    /// Список координат географических объектов
    /// </summary>
    public List<GeographyObjectCoordinatesResponseList>? Items { get; set; } = items;
}