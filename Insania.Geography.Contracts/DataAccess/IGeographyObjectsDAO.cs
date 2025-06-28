using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными географических объектов
/// </summary>
public interface IGeographyObjectsDAO
{
    /// <summary>
    /// Метод получения списка географических объектов
    /// </summary>
    /// <returns cref="List{GeographyObject}">Список географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<GeographyObject>> GetList();
}