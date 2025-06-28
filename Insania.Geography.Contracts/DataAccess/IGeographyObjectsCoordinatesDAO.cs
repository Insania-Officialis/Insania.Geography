using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными координат географических объектов
/// </summary>
public interface IGeographyObjectsCoordinatesDAO
{
    /// <summary>
    /// Метод получения списка координат географических объектов
    /// </summary>
    /// <returns cref="List{GeographyObjectCoordinate}">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<GeographyObjectCoordinate>> GetList();
}