using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;
using Insania.Geography.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Сервис работы с данными типов координат
/// </summary>
/// <param cref="ILogger{CoordinatesTypesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных географии</param>
public class CoordinatesTypesDAO(ILogger<CoordinatesTypesDAO> logger, GeographyContext context) : ICoordinatesTypesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<CoordinatesTypesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка типов координат
    /// </summary>
    /// <returns cref="List{CoordinateTypeGeography}">Список типов координат</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<CoordinateTypeGeography>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListCoordinatesTypesMethod);

            //Получение данных из бд
            List<CoordinateTypeGeography> data = await _context.CoordinatesTypes
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