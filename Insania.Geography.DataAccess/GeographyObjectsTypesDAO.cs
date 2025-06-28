using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Database.Contexts;
using Insania.Geography.Entities;
using Insania.Geography.Messages;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Сервис работы с данными типов географических объектов
/// </summary>
/// <param cref="ILogger{GeographyTypesDAO}" name="logger">Сервис логгирования</param>
/// <param cref="GeographyContext" name="context">Контекст базы данных географии</param>
public class GeographyObjectsTypesDAO(ILogger<GeographyObjectsTypesDAO> logger, GeographyContext context) : IGeographyObjectsTypesDAO
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsTypesDAO> _logger = logger;

    /// <summary>
    /// Контекст базы данных географии
    /// </summary>
    private readonly GeographyContext _context = context;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка типов географических объектов
    /// </summary>
    /// <returns cref="List{GeographyObjectType}">Список типов географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<GeographyObjectType>> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsTypesMethod);

            //Получение данных из бд
            List<GeographyObjectType> data = await _context.GeographyObjectsTypes.Where(x => x.DateDeleted == null).ToListAsync();

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