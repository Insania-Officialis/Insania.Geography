using Insania.Shared.Models.Responses.Base;

namespace Insania.Geography.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой географических объектов
/// </summary>
public interface IGeographyObjectsBL
{
    /// <summary>
    /// Метод получения списка географических объектов
    /// </summary>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <remarks>Список географических объектов</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList> GetList();
}