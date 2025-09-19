using AutoMapper;
using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Models.Responses.GeographyObjects;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;
using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System.Transactions;
using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;
using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.Geography.Messages.InformationMessages;

namespace Insania.Geography.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой географических объектов
/// </summary>
/// <param cref="ILogger{GeographyObjectsBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="IGeographyObjectsDAO" name="geographyObjectsDAO">Сервис работы с данными географических объектов</param>
/// <param cref="IPolygonParserSL" name="polygonParserSL">Сервис преобразования полигона</param>
public class GeographyObjectsBL(ILogger<GeographyObjectsBL> logger, IMapper mapper, IGeographyObjectsDAO geographyObjectsDAO, IPolygonParserSL polygonParserSL) : IGeographyObjectsBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными географических объектов
    /// </summary>
    private readonly IGeographyObjectsDAO _geographyObjectsDAO = geographyObjectsDAO;

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private readonly IPolygonParserSL _polygonParserSL = polygonParserSL;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка географических объектов
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <param cref="long?" name="typeId">Идентификатор типа</param>
    /// <param cref="long[]?" name="typeIds">Идентификаторы типов</param>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList> GetList(bool? hasCoordinates = null, long? typeId = null, long[]? typeIds = null)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsMethod);

            //Получение данных
            List<GeographyObject>? data = await _geographyObjectsDAO.GetList(hasCoordinates, typeId, typeIds);

            //Формирование ответа
            BaseResponseList? response = null;
            if (data == null) response = new(false, null);
            else response = new(true, data?.Select(_mapper.Map<BaseResponseListItem>).ToList());

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
    /// Метод получения списка географических объектов с координатами
    /// </summary>
    /// <param cref="long[]?" name="typeIds">Идентификаторы типов</param>
    /// <returns cref="GeographyObjectsWithCoordinatesResponseList">Список географических объектов с координатами</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<GeographyObjectsWithCoordinatesResponseList> GetListWithCoordinates(long[]? typeIds = null)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsCoordinatesMethod);

            //Получение данных
            List<GeographyObject>? data = await _geographyObjectsDAO.GetList(hasCoordinates: true, typeIds: typeIds);

            //Формирование ответа
            GeographyObjectsWithCoordinatesResponseList? response = null;
            if (data == null) response = new(false);
            else response = new(
                true,
                [
                    .. data.Select(
                        x => new GeographyObjectsWithCoordinatesResponseListItem(
                            x.Id,
                            x.Name,
                            [
                                x.GeographyObjectCoordinates?.OrderByDescending(y => y.Area)?.FirstOrDefault()?.Center.X,
                                x.GeographyObjectCoordinates?.OrderByDescending(y => y.Area)?.FirstOrDefault()?.Center.Y
                            ],
                            x.GeographyObjectCoordinates?.OrderByDescending(y => y.Area)?.FirstOrDefault()?.Zoom,
                            [
                                ..x.GeographyObjectCoordinates?.Select(
                                    y => new GeographyObjectCoordinatesResponseListItem(
                                        y.Id,
                                        y.CoordinateId,
                                        _polygonParserSL.FromPolygonToDoubleArray(y.CoordinateEntity?.PolygonEntity),
                                        y.CoordinateEntity?.TypeEntity?.BackgroundColor,
                                        y.CoordinateEntity?.TypeEntity?.BorderColor
                                    )
                                ) ?? []
                            ]
                        )
                    )
                ]
            );

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