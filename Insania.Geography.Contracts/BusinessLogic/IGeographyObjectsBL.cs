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
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <param cref="long?" name="typeId">Идентификатор типа</param>
    /// <param cref="long[]?" name="typeIds">Идентификаторы типов</param>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <remarks>Список географических объектов</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponseList> GetList(bool? hasCoordinates = null, long? typeId = null, long[]? typeIds = null);
}