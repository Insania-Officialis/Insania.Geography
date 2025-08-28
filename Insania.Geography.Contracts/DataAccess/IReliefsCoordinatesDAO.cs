using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными координат рельефов
/// </summary>
public interface IReliefsCoordinatesDAO
{
    /// <summary>
    /// Метод получения списка координат рельефов
    /// </summary>
    /// <returns cref="List{ReliefCoordinate}">Список координат рельефов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<ReliefCoordinate>> GetList();
}