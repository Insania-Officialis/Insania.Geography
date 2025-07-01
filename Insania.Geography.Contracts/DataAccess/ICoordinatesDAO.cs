using NetTopologySuite.Geometries;

using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными координат
/// </summary>
public interface ICoordinatesDAO
{
    /// <summary>
    /// Метод получения координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <returns cref="CoordinateGeography?">Координата</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<CoordinateGeography?> GetById(long? id);

    /// <summary>
    /// Метод получения списка координат
    /// </summary>
    /// <returns cref="List{CoordinateGeography}">Список координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CoordinateGeography>> GetList();

    /// <summary>
    /// Метод изменения координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <param cref="Polygon?" name="coordinates">Координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Edit(long? id, Polygon? coordinates, string username);
}