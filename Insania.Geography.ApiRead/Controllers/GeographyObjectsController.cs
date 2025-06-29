using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;

namespace Insania.Geography.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с географическими объектами
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IGeographyObjectsBL" name="geographyObjectsBL">Сервис работы с бизнес-логикой географических объектов</param>
[Route("geography_objects")]
public class GeographyObjectsController(ILogger<GeographyObjectsController> logger, IGeographyObjectsBL geographyObjectsBL) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsController> _logger = logger;

    /// <summary>
    /// Сервис работы с бизнес-логикой географических объектов
    /// </summary>
    private readonly IGeographyObjectsBL _geographyObjectsBL = geographyObjectsBL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка географических объектов
    /// </summary>
    /// <returns cref="OkResult">Список географических объектов</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            //Получение результата
            BaseResponse? result = await _geographyObjectsBL.GetList();

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