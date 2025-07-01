using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using NetTopologySuite.Geometries;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;
using InformationMessages = Insania.Geography.Messages.InformationMessages;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Сервис работы с данными координат
/// </summary>
/// <param cref="ILogger{CoordinatesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных географии</param>
public class CoordinatesDAO(ILogger<CoordinatesDAO> logger, GeographyContext context) : ICoordinatesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CoordinatesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <returns cref="CoordinateGeography?">Координата</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<CoordinateGeography?> GetById(long? id)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByIdCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);

            //Получение данных из бд
            CoordinateGeography? data = await _context.Coordinates
                .Where(x => x.Id == id)
                .Include(x => x.TypeEntity)
                .FirstOrDefaultAsync();

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
    /// Метод получения списка координат
    /// </summary>
    /// <returns cref="List{CoordinateGeography}">Список координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<CoordinateGeography>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCoordinatesMethod);

            //Получение данных из бд
            List<CoordinateGeography> data = await _context.Coordinates
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
    /// Метод изменения координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <param cref="Polygon?" name="coordinates">Координаты</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<bool> Edit(long? id, Polygon? coordinates, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredEditCoordinateMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);
            if (coordinates == null) throw new Exception(ErrorMessagesShared.EmptyCoordinates);

            //Получение данных из бд
            CoordinateGeography data = await GetById(id) ?? throw new Exception(ErrorMessagesGeography.NotFoundCoordinate);

            //Проверки
            if (data.DateDeleted != null) throw new Exception(ErrorMessagesGeography.DeletedCoordinate);
            if (data.PolygonEntity == coordinates) throw new Exception(ErrorMessagesGeography.NotChangesCoordinate);

            //Запись данных в бд
            data.SetPolygon(coordinates);
            data.SetUpdate(username);
            _context.Update(data);
            await _context.SaveChangesAsync();

            //Возврат результата
            return true;
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