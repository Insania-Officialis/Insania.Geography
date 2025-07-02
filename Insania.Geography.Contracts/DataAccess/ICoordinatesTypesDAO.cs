using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными типов координат
/// </summary>
public interface ICoordinatesTypesDAO
{
    /// <summary>
    /// Метод получения типа координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор типа координаты</param>
    /// <returns cref="CoordinateTypeGeography?">Тип координаты</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<CoordinateTypeGeography?> GetById(long? id);

    /// <summary>
    /// Метод получения списка типов координат
    /// </summary>
    /// <returns cref="List{CoordinateTypeGeography}">Список типов координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CoordinateTypeGeography>> GetList();
}