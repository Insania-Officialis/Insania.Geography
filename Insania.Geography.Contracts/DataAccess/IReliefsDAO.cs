using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными рельефов
/// </summary>
public interface IReliefsDAO
{
    /// <summary>
    /// Метод получения списка рельефов
    /// </summary>
    /// <returns cref="List{Relief}">Список рельефов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<Relief>> GetList();
}