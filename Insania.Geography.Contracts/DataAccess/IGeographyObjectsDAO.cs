using Insania.Geography.Entities;

namespace Insania.Geography.Contracts.DataAccess;

/// <summary>
/// Интерфейс работы с данными географических объектов
/// </summary>
public interface IGeographyObjectsDAO
{
    /// <summary>
    /// Метод получения географического объекта по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    /// <returns cref="GeographyObject?">Географический объект</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<GeographyObject?> GetById(long? id);

    /// <summary>
    /// Метод получения списка географических объектов
    /// </summary>
    /// <returns cref="List{GeographyObject}">Список географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<List<GeographyObject>> GetList();

    /// <summary>
    /// Метод восстановления географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Restore(long? id, string username);

    /// <summary>
    /// Метод закрытия географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    Task<bool> Close(long? id, string username);
}