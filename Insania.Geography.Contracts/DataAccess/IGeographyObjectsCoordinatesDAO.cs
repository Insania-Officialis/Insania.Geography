using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными координат географических объектов
/// </summary>
public interface IGeographyObjectsCoordinatesDAO
{
    /// <summary>
    /// Метод получения координаты географического объекта по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты географического объекта</param>
    /// <returns cref="GeographyObjectCoordinate?">Координата географического объекта</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<GeographyObjectCoordinate?> GetById(long? id);

    /// <summary>
    /// Метод получения координаты географического объекта по идентификаторам географического объекта и координаты
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <returns cref="GeographyObjectCoordinate?">Координата географического объекта</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<GeographyObjectCoordinate?> GetByGeographyObjectIdAndCoordinateId(long? geographyObjectId, long? coordinateId);

    /// <summary>
    /// Метод получения списка координат географических объектов
    /// </summary>
    /// <returns cref="List{GeographyObjectCoordinate}">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<GeographyObjectCoordinate>> GetList();

    /// <summary>
    /// Метод получения списка координат географических объектов по идентификатору географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <returns cref="List{GeographyObjectCoordinate}">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<GeographyObjectCoordinate>> GetList(long? geographyObjectId);

    /// <summary>
    /// Метод добавления координаты географического объекта
    /// </summary>
    /// <param cref="GeographyObject?" name="geographyObject">Географический объект</param>
    /// <param cref="CoordinateGeography?" name="coordinate">Координаты</param>
    /// <param cref="int?" name="zoom">Коэффициент масштаба отображения сущности</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="long?">Идентификатор координаты географического объекта</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<long?> Add(GeographyObject? geographyObject, CoordinateGeography? coordinate, int? zoom, string username);

    /// <summary>
    /// Метод восстановления координаты географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты географического объекта</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Restore(long? id, string username);

    /// <summary>
    /// Метод закрытия координаты географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты географического объекта</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Close(long? id, string username);
}