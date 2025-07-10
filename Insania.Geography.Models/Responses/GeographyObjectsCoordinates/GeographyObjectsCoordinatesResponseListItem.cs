using NetTopologySuite.Geometries;

namespace Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

/// <summary>
/// Модель элемента ответа списком координат географических объектов
/// </summary>
public class GeographyObjectsCoordinatesResponseListItem
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели элемента ответа списком координат географических объектов
    /// </summary>
    public GeographyObjectsCoordinatesResponseListItem()
    {

    }

    /// <summary>
    /// Конструктор модели элемента ответа списком координат географических объектов
    /// </summary>
    /// <param cref="double[][][]?" name="coordinates">Координаты</param>
    public GeographyObjectsCoordinatesResponseListItem(double[][][]? coordinates)
    {
        Coordinates = coordinates;
    }
    #endregion

    #region Поля
    /// <summary>
    /// Координаты
    /// </summary>
    public double[][][]? Coordinates { get; set; }
    #endregion
}