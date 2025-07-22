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
    /// <param cref="bool" name="has_coordinates">Проверка наличия координат</param>
    /// <param cref="long" name="type_id">Идентификатор типа</param>
    /// <param cref="IEnumerable{Int64}" name="type_ids">Идентификаторы типов</param>
    /// <returns cref="OkResult">Список географических объектов</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetList([FromQuery] bool? has_coordinates = null, [FromQuery] long? type_id = null, [FromQuery] long[]? type_ids = null)
    {
        try
        {
            //Получение результата
            BaseResponse? result = await _geographyObjectsBL.GetList(has_coordinates, type_id, type_ids?.ToArray());

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