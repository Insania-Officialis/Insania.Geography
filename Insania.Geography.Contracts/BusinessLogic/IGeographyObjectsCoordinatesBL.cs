using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Models.Requests.GeographyObjectsCoordinates;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

namespace Insania.Geography.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой координат географических объектов
/// </summary>
public interface IGeographyObjectsCoordinatesBL
{
    /// <summary>
    /// Метод получения списка координат географического объекта по идентификатору географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <returns cref="GeographyObjectCoordinatesResponseList">Список координат географического объекта</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<GeographyObjectCoordinatesResponseList> GetByGeographyObjectId(long? geographyObjectId);

    /// <summary>
    /// Метод получения списка координат географических объектов
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <param cref="long[]?" name="typeIds">Идентификаторы типов</param>
    /// <returns cref="GeographyObjectsCoordinatesResponseList">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<GeographyObjectsCoordinatesResponseList> GetList(bool? hasCoordinates = null, long[]? typeIds = null);

    /// <summary>
    /// Метод актуализации координаты географического объекта
    /// </summary>
    /// <param cref="GeographyObjectsCoordinatesUpgradeRequest?" name="request">Модель запроса актуализации координаты географического объекта</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>Новый идентификатор координаты географического объекта</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponse> Upgrade(GeographyObjectsCoordinatesUpgradeRequest? request, string username);
}