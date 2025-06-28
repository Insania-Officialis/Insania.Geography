using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными координат
/// </summary>
public interface ICoordinatesDAO
{
    /// <summary>
    /// Метод получения списка координат
    /// </summary>
    /// <returns cref="List{CoordinateGeography}">Список координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<CoordinateGeography>> GetList();
}