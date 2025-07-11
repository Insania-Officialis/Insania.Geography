namespace Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

/// <summary>
/// Модель элемента ответа списком координат географических объектов
/// </summary>
/// <param cref="long?" name="id">Идентификатор координаты географического объекта</param>
/// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
/// <param cref="double[][][]?" name="coordinates">Координаты</param>
public class GeographyObjectsCoordinatesResponseListItem(long? id = null, long? coordinateId = null, double[][][]? coordinates = null)
{
    #region Поля
    /// <summary>
    /// Идентификатор координаты географического объекта
    /// </summary>
    public long? Id { get; set; } = id;

    /// <summary>
    /// Идентификатор координаты
    /// </summary>
    public long? CoordinateId { get; set; } = coordinateId;

    /// <summary>
    /// Координаты
    /// </summary>
    public double[][][]? Coordinates { get; set; } = coordinates;
    #endregion
}