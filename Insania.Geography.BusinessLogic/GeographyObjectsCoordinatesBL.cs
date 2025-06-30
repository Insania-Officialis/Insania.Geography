using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;
using Insania.Geography.Entities;

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
public class GeographyObjectsCoordinatesBL(ILogger<GeographyObjectsCoordinatesBL> logger, IMapper mapper, IGeographyObjectsCoordinatesDAO geographyObjectsCoordinatesDAO) : IGeographyObjectsCoordinatesBL
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
            List<CoordinateGeography?> coordinates = [.. data.Select(x => x.CoordinateEntity as CoordinateGeography)];

            //Формирование ответа
            GeographyObjectsCoordinatesResponseList? response = null;
            if (data == null) response = new(false);
            else response = new(true, geographyObject.Name, geographyObjectCoordinate.Center, geographyObjectCoordinate.Zoom, coordinates?.Select(_mapper.Map<GeographyObjectsCoordinatesResponseListItem>).ToList());

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