using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using InformationMessages = Insania.Geography.Messages.InformationMessages;
using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Сервис работы с данными координат географических объектов
/// </summary>
/// <param cref="ILogger{GeographyObjectsCoordinatesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных географии</param>
public class GeographyObjectsCoordinatesDAO(ILogger<GeographyObjectsCoordinatesDAO> logger, GeographyContext context) : IGeographyObjectsCoordinatesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsCoordinatesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка координат географических объектов
    /// </summary>
    /// <returns cref="List{GeographyObjectCoordinate}">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<GeographyObjectCoordinate>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsCoordinatesMethod);

            //Получение данных из бд
            List<GeographyObjectCoordinate> data = await _context.GeographyObjectsCoordinates
                .Where(x => x.DateDeleted == null)
                .ToListAsync();

            //Возврат результата
            return data;
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
    /// Метод получения списка координат географических объектов по идентификатору географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <returns cref="List{GeographyObjectCoordinate}">Список координат географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<GeographyObjectCoordinate>> GetList(long? geographyObjectId)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsCoordinatesMethod);

            //Проверки
            if (geographyObjectId == null) throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Получение данных из бд
            List<GeographyObjectCoordinate> data = await _context.GeographyObjectsCoordinates
                .Include(x => x.GeographyObjectEntity)
                .Include(x => x.CoordinateEntity)
                .Where(x => x.DateDeleted == null && x.GeographyObjectId == geographyObjectId)
                .ToListAsync();

            //Возврат результата
            return data;
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