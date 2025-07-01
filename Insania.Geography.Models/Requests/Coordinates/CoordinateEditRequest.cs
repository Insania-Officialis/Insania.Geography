namespace Insania.Geography.Models.Requests.Coordinates;

/// <summary>
/// Модель запроса изменения координаты
/// </summary>
/// <param cref="long?" name="id">Идентификатор</param>
/// <param cref="double[][][]?" name="coordinates">Координаты</param>
public class CoordinateEditRequest(long? id = null, double[][][]? coordinates = null)
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public long? Id { get; set; } = id;

    /// <summary>
    /// Координаты
    /// </summary>
    public double[][][]? Coordinates { get; set; } = coordinates;
}