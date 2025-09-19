using Microsoft.AspNetCore.Mvc;

using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

namespace Insania.Geography.ApiRead.Controllers;

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
    /// Метод получения списка координат географических объектов по идентификатору географического объекта
    /// </summary>
    /// <param cref="long" name="geography_object_id">Идентификатор географического объекта</param>
    /// <returns cref="OkResult">Список координат географических объектов</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("by_geography_object_id")]
    public async Task<IActionResult> GetByGeographyObjectId([FromQuery] long? geography_object_id)
    {
        try
        {
            //Получение результата
            GeographyObjectCoordinatesResponseList? result = await _geographyObjectsCoordinatesBL.GetByGeographyObjectId(geography_object_id);

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