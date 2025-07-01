using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Models.Requests.Coordinates;

namespace Insania.Geography.Contracts.BusinessLogic;

/// <summary>
/// Интерфейс работы с бизнес-логикой координат
/// </summary>
public interface ICoordinatesBL
{
    /// <summary>
    /// Метод изменения координаты
    /// </summary>
    /// <param cref="CoordinateEditRequest?" name="request">Запрос</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>Признак успешности</remarks>
    /// <exception cref="Exception">Исключение</exception>
    Task<BaseResponse> Edit(CoordinateEditRequest? request, string username);
}