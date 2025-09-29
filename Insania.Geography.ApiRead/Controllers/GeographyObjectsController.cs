using Insania.Geography.Contracts.BusinessLogic;
using Insania.Shared.Messages;
using Insania.Shared.Models.Responses.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Insania.Geography.ApiRead.Controllers;

/// <summary>
/// Контроллер работы с географическими объектами
/// </summary>
/// <param cref="ILogger" name="logger">Сервис логгирования</param>
/// <param cref="IMemoryCache" name="memoryCache">Сервис кэширования</param>
/// <param cref="IGeographyObjectsBL" name="geographyObjectsBL">Сервис работы с бизнес-логикой географических объектов</param>
[Route("geography_objects")]
public class GeographyObjectsController(ILogger<GeographyObjectsController> logger, IMemoryCache memoryCache, IGeographyObjectsBL geographyObjectsBL) : Controller
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsController> _logger = logger;

    /// <summary>
    /// Сервис кэширования
    /// </summary>
    private readonly IMemoryCache _memoryCache = memoryCache;

    /// <summary>
    /// Сервис работы с бизнес-логикой географических объектов
    /// </summary>
    private readonly IGeographyObjectsBL _geographyObjectsBL = geographyObjectsBL;
    #endregion

    #region Поля
    /// <summary>
    /// Класс для синхронизации потоков
    /// </summary>
    private static readonly SemaphoreSlim _cacheSemaphore = new(1, 1);
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

    /// <summary>
    /// Метод получения списка географических объектов с координатами
    /// </summary>
    /// <param cref="IEnumerable{Int64}" name="type_ids">Идентификаторы типов</param>
    /// <returns cref="OkResult">Список географических объектов с координатами</returns>
    /// <returns cref="BadRequestResult">Ошибка</returns>
    [HttpGet]
    [Route("list_with_coordinates")]
    public async Task<IActionResult> GetListWithCoordinates([FromQuery] long[]? type_ids = null)
    {
        try
        {
            //Формирование ключа кэша
            string cacheKey = $"geo_objects_{string.Join("_", type_ids ?? [])}";

            //Возврат результата при его наличии в кэше
            if (_memoryCache.TryGetValue(cacheKey, out string? cachedResult) && cachedResult != null) return Content(cachedResult, "application/json");

            //Установка блокировки
            await _cacheSemaphore.WaitAsync();
            try
            {
                //Возврат результата при его наличии в кэше после установки блокировки
                if (_memoryCache.TryGetValue(cacheKey, out cachedResult) && cachedResult != null) return Content(cachedResult, "application/json");

                //Получение результата
                BaseResponse? result = await _geographyObjectsBL.GetListWithCoordinates(type_ids?.ToArray());

                //Сериализация ответа
                JsonSerializerSettings settings = new()
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Formatting = Formatting.None
                };
                string serializedResult = JsonConvert.SerializeObject(result, settings);

                //Запись в кэш
                _memoryCache.Set(cacheKey, serializedResult, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), Size = 1 });

                //Возврат результата
                return Content(serializedResult, "application/json");
            }
            finally
            {
                //Освобождение потока
                _cacheSemaphore.Release();
            }
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