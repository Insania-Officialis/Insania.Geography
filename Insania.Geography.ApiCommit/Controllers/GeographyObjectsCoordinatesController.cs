using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Models.Requests.GeographyObjectsCoordinates;

namespace Insania.Geography.ApiCommit.Controllers;

/// <summary>
/// Контроллер работы с координатами географических объектов
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IGeographyObjectsCoordinatesBL" name="geographyObjectsCoordinatesBL">Сервис работы с бизнес-логикой координат географических объектов</param>
[Route("geography_objects_coordinates")]
public class GeographyObjectsCoordinatesController(ILogger<GeographyObjectsCoordinatesController> logger, IGeographyObjectsCoordinatesBL geographyObjectsCoordinatesBL) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsCoordinatesController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой координат географических объектов
    /// </summary>
    private readonly IGeographyObjectsCoordinatesBL _geographyObjectsCoordinatesBL = geographyObjectsCoordinatesBL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод актуализации координаты географического объекта
    /// </summary>
    /// <param cref="GeographyObjectsCoordinatesUpgradeRequest" name="request">Модель запроса актуализации координаты географического объекта</param>
    /// <returns cref="OkResult">Список координат географических объектов</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpPost]
    [Route("upgrade")]
    public async Task<IActionResult> Upgrade([FromBody] GeographyObjectsCoordinatesUpgradeRequest? request)
    {
        try
        {
            //Проверки
            if (request == null) throw new Exception(ErrorMessages.EmptyRequest);

            //Получение текущего пользователя
            string username = User?.Identity?.Name ?? throw new Exception(ErrorMessages.NotFoundCurrentUser);

            //Получение результата
            BaseResponse? result = await _geographyObjectsCoordinatesBL.Upgrade(request, username);

            //Возврат ответа
            return Ok(result);
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text} {ex}", ErrorMessages.Error, ex);

            //Возврат ошибки
            return BadRequest(new BaseResponseError(ex.Message));
        }
    }
    #endregion
}