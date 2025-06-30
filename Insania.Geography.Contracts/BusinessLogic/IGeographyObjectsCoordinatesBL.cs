using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

namespace Insania.Geography.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой координат географических объектов
/// </summary>
public interface IGeographyObjectsCoordinatesBL
{
    /// <summary>
    /// Метод получения списка координат географических объектов по идентификатору географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <returns cref="GeographyObjectsCoordinatesResponseList">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<GeographyObjectsCoordinatesResponseList> GetList(long? geographyObjectId);
}