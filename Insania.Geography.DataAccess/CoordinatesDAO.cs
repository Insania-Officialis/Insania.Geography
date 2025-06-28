using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;
using Insania.Geography.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

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
            _logger.LogError("{text}: {error}", ErrorMessages.Error, ex.Message);

            //Проброс исключения
            throw;
        }
    }
    #endregion
}