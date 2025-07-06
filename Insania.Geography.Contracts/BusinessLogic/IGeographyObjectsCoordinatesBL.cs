using NetTopologySuite.Geometries;

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

    /// <summary>
    /// Метод актуализации координаты географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <param cref="double[][][]?" name="coordinates">Координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="long?">Новый идентификатор координаты географического объекта</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<long?> Upgrade(long? geographyObjectId, long? coordinateId, double[][][]? coordinates, string username);
}