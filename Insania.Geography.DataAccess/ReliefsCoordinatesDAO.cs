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
/// Сервис работы с данными координат рельефов
/// </summary>
/// <param cref="ILogger{ReliefsCoordinatesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных географии</param>
public class ReliefsCoordinatesDAO(ILogger<ReliefsCoordinatesDAO> logger, GeographyContext context) : IReliefsCoordinatesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ReliefsCoordinatesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка координат рельефов
    /// </summary>
    /// <returns cref="List{ReliefCoordinate}">Список координат рельефов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<ReliefCoordinate>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListReliefsCoordinatesMethod);

            //Получение данных из бд
            List<ReliefCoordinate> data = await _context.ReliefsCoordinates
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
    #endregion
}