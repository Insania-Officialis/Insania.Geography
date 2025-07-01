using Microsoft.Extensions.Logging;

using AutoMapper;
using NetTopologySuite.Geometries;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Models.Requests.Coordinates;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;
using InformationMessages = Insania.Geography.Messages.InformationMessages;

namespace Insania.Geography.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой координат
/// </summary>
/// <param cref="ILogger{CoordinatesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="ICoordinatesDAO" name="coordinatesDAO">Сервис работы с данными координат</param>
/// <param cref="IPolygonParserSL" name="polygonParserSL">Сервис преобразования полигона</param>
public class CoordinatesBL(ILogger<CoordinatesBL> logger, IMapper mapper, ICoordinatesDAO coordinatesDAO, IPolygonParserSL polygonParserSL) : ICoordinatesBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CoordinatesBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными координат
    /// </summary>
    private readonly ICoordinatesDAO _coordinatesDAO = coordinatesDAO;

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private readonly IPolygonParserSL _polygonParserSL = polygonParserSL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод изменения координаты
    /// </summary>
    /// <param cref="CoordinateEditRequest?" name="request">Запрос</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>Признак успешности</remarks>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponse> Edit(CoordinateEditRequest? request, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCoordinatesMethod);

            //Проверки
            if (request == null) throw new Exception(ErrorMessagesShared.EmptyRequest);
            if (request.Id == null) throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);
            if (request.Coordinates == null) throw new Exception(ErrorMessagesShared.EmptyCoordinates);

            //Формирование запроса
            Polygon? polygon = _polygonParserSL.FromDoubleArrayToPolygon(request.Coordinates);

            //Получение данных
            bool result = await _coordinatesDAO.Edit(request.Id, polygon, username);

            //Формирование ответа
            BaseResponse? response = new(result);

            //Возврат ответа
            return response;
        }
        catch (Exception ex)
        {
            //Логгирование
            _logger.LogError("{text}: {error}", ErrorMessagesShared.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}