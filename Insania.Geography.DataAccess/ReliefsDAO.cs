using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;
using InformationMessages = Insania.Geography.Messages.InformationMessages;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Сервис работы с данными рельефов
/// </summary>
/// <param cref="ILogger{ReliefsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных географии</param>
public class ReliefsDAO(ILogger<ReliefsDAO> logger, GeographyContext context) : IReliefsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<ReliefsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка рельефов
    /// </summary>
    /// <returns cref="List{Relief}">Список рельефов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<Relief>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListReliefsMethod);

            //Формирование запроса
            IQueryable<Relief> query = _context.Reliefs.Where(x => x.DateDeleted == null);

            //Получение данных из бд
            List<Relief> data = await query.ToListAsync();

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