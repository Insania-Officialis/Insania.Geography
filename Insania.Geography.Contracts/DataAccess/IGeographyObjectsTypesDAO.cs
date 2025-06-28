using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данным типов географических объектов
/// </summary>
public interface IGeographyObjectsTypesDAO
{
    /// <summary>
    /// Метод получения списка типов географических объектов
    /// </summary>
    /// <returns cref="List{OrganizationType}">Список типов географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<GeographyObjectType>> GetList();
}