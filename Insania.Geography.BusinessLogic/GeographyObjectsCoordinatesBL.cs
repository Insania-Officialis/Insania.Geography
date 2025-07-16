using System.Transactions;

using Microsoft.Extensions.Logging;

using AutoMapper;
using NetTopologySuite.Geometries;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;
using Insania.Geography.Models.Requests.GeographyObjectsCoordinates;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;
using InformationMessages = Insania.Geography.Messages.InformationMessages;

namespace Insania.Geography.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой координат географических объектов
/// </summary>
/// <param cref="ILogger{GeographyObjectsCoordinatesBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="IGeographyObjectsCoordinatesDAO" name="geographyObjectsCoordinatesDAO">Сервис работы с данными координат географических объектов</param>
/// <param cref="ICoordinatesDAO" name="coordinatesDAO">Сервис работы с данными координат</param>
/// <param cref="IGeographyObjectsDAO" name="geographyObjectsDAO">Сервис работы с данными географических объектов</param>
/// <param cref="IPolygonParserSL" name="polygonParserSL">Сервис преобразования полигона</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных (только для управления транзакциями)</param>
public class GeographyObjectsCoordinatesBL(ILogger<GeographyObjectsCoordinatesBL> logger, IMapper mapper, IGeographyObjectsCoordinatesDAO geographyObjectsCoordinatesDAO, ICoordinatesDAO coordinatesDAO, IGeographyObjectsDAO geographyObjectsDAO, IPolygonParserSL polygonParserSL, GeographyContext context) : IGeographyObjectsCoordinatesBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsCoordinatesBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными координат географических объектов
    /// </summary>
    private readonly IGeographyObjectsCoordinatesDAO _geographyObjectsCoordinatesDAO = geographyObjectsCoordinatesDAO;

    /// <summary>
    /// Сервис работы с данными координат
    /// </summary>
    private readonly ICoordinatesDAO _coordinatesDAO = coordinatesDAO;

    /// <summary>
    /// Сервис работы с данными географических объектов
    /// </summary>
    private readonly IGeographyObjectsDAO _geographyObjectsDAO = geographyObjectsDAO;

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private readonly IPolygonParserSL _polygonParserSL = polygonParserSL;

    /// <summary>
    /// Контекст базы данных (только для управления транзакциями)
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка координат географических объектов по идентификатору географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <returns cref="GeographyObjectsCoordinatesResponseList">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<GeographyObjectsCoordinatesResponseList> GetList(long? geographyObjectId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsCoordinatesMethod);

            //Проверки
            if (geographyObjectId == null) throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Получение данных
            List<GeographyObjectCoordinate>? data = await _geographyObjectsCoordinatesDAO.GetList(geographyObjectId);
            GeographyObjectCoordinate geographyObjectCoordinate = data?
                .OrderByDescending(x => x.Area)
                .FirstOrDefault() ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObjectCoordinate);
            GeographyObject geographyObject = geographyObjectCoordinate.GeographyObjectEntity ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Формирование ответа
            GeographyObjectsCoordinatesResponseList? response = null;
            if (data == null) response = new(false);
            else response = new(true, geographyObject.Id, geographyObject.Name, geographyObjectCoordinate.Center, geographyObjectCoordinate.Zoom, data?.Select(x => new GeographyObjectsCoordinatesResponseListItem(x.Id, x.CoordinateId, _polygonParserSL.FromPolygonToDoubleArray(x?.CoordinateEntity?.PolygonEntity), x?.CoordinateEntity?.TypeEntity?.BackgroundColor, x?.CoordinateEntity?.TypeEntity?.BorderColor)).ToList());

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

    /// <summary>
    /// Метод актуализации координаты географического объекта
    /// </summary>
    /// <param cref="GeographyObjectsCoordinatesUpgradeRequest?" name="request">Модель запроса актуализации координаты географического объекта</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <param cref="double[][][]?" name="coordinates">Координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="BaseResponse">Стандартный ответ</returns>
    /// <remarks>Новый идентификатор координаты географического объекта</remarks>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponse> Upgrade(GeographyObjectsCoordinatesUpgradeRequest? request, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredUpgradeGeographyObjectCoordinateMethod);

            //Проверки
            if (request == null) throw new Exception(ErrorMessagesShared.EmptyRequest);
            if (request.GeographyObjectId == null) throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);
            if (request.CoordinateId == null) throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);
            if (request.Coordinates == null) throw new Exception(ErrorMessagesShared.EmptyCoordinates);
            Polygon polygon = _polygonParserSL.FromDoubleArrayToPolygon(request.Coordinates) ?? throw new Exception(ErrorMessagesShared.IncorrectCoordinates);

            //Получение данных
            GeographyObject geographyObject = await _geographyObjectsDAO.GetById(request.GeographyObjectId) ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);
            CoordinateGeography coordinate = await _coordinatesDAO.GetById(request.CoordinateId) ?? throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);

            //Проверки
            if (geographyObject.DateDeleted != null) throw new Exception(ErrorMessagesGeography.DeletedGeographyObject);
            if (coordinate.DateDeleted != null) throw new Exception(ErrorMessagesGeography.DeletedCoordinate);
            if (coordinate.PolygonEntity == polygon) throw new Exception(ErrorMessagesGeography.NotChangesCoordinate);

            //Получение данных
            GeographyObjectCoordinate geographyObjectCoordinateOld = await _geographyObjectsCoordinatesDAO.GetByGeographyObjectIdAndCoordinateId(request.GeographyObjectId, request.CoordinateId) ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObjectCoordinate);

            //Проверки
            if (geographyObjectCoordinateOld.DateDeleted != null) throw new Exception(ErrorMessagesGeography.DeletedGeographyObjectCoordinate);

            //Открытие транзакции
            using TransactionScope transactionScope = new(
                TransactionScopeOption.Required, 
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TransactionManager.DefaultTimeout },
                TransactionScopeAsyncFlowOption.Enabled
            );

            //Закрытие старой координаты географического объекта
            await _geographyObjectsCoordinatesDAO.Close(geographyObjectCoordinateOld.Id, username);

            //Создание новой координаты
            long? coordinateIdNew = await _coordinatesDAO.Add(polygon, geographyObjectCoordinateOld.CoordinateEntity?.TypeEntity as CoordinateTypeGeography, username);

            //Получение сущностей
            CoordinateGeography coordinateNew = await _coordinatesDAO.GetById(coordinateIdNew) ?? throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);

            //Создание новой координаты географического объекта
            long? result = await _geographyObjectsCoordinatesDAO.Add(geographyObject, coordinateNew, geographyObjectCoordinateOld.Zoom, username);

            //Фиксация транзакции
            transactionScope.Complete();

            //Возврат ответа
            return new(true, result);
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