using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;
using Insania.Geography.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Сервис работы с данными географических объектов
/// </summary>
/// <param cref="ILogger{GeographyObjectsDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных географии</param>
public class GeographyObjectsDAO(ILogger<GeographyObjectsDAO> logger, GeographyContext context) : IGeographyObjectsDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка географических объектов
    /// </summary>
    /// <returns cref="List{GeographyObject}">Список географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<GeographyObject>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsMethod);

            //Получение данных из бд
            List<GeographyObject> data = await _context.GeographyObjects
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