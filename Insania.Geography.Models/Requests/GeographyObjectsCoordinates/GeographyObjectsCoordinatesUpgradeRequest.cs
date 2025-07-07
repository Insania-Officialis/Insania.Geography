using System.Text.Json.Serialization;

namespace Insania.Geography.Models.Requests.GeographyObjectsCoordinates;

/// <summary>
/// Модель запроса актуализации координаты географического объекта
/// </summary>
/// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
/// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
/// <param cref="string?" name="coordinates">Координаты</param>
public class GeographyObjectsCoordinatesUpgradeRequest(long? geographyObjectId, long? coordinateId, double[][][]? coordinates)
{
    /// <summary>
    /// Идентификатор географического объекта
    /// </summary>
    [JsonPropertyName("geography_object_id")]
    public long? GeographyObjectId { get; set; } = geographyObjectId;

    /// <summary>
    /// Идентификатор координаты
    /// </summary>
    [JsonPropertyName("coordinate_id")]
    public long? CoordinateId { get; set; } = coordinateId;

    /// <summary>
    /// Координаты
    /// </summary>
    [JsonPropertyName("coordinates")]
    public double[][][]? Coordinates { get; set; } = coordinates;
}