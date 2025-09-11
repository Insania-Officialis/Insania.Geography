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
    /// Метод получения географического объекта по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    /// <returns cref="GeographyObject?">Географический объект</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<GeographyObject?> GetById(long? id)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetByIdGeographyObjectMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Получение данных из бд
            GeographyObject? data = await _context.GeographyObjects
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
    /// Метод получения списка географических объектов
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    /// <param cref="long?" name="typeId">Идентификатор типа</param>
    /// <param cref="long[]?" name="typeIds">Идентификаторы типов</param>
    /// <returns cref="List{GeographyObject}">Список географических объектов</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<List<GeographyObject>> GetList(bool? hasCoordinates = null, long? typeId = null, long[]? typeIds = null)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsMethod);

            //Формирование запроса
            IQueryable<GeographyObject> query = _context.GeographyObjects.Where(x => x.DateDeleted == null);
            if (hasCoordinates.HasValue) query = query
                    .Include(x => x.GeographyObjectCoordinates)
                    .Where(x => (hasCoordinates ?? false)
                        ? x.GeographyObjectCoordinates != null &&
                          x.GeographyObjectCoordinates.Any(y => y.DateDeleted == null)
                        : x.GeographyObjectCoordinates == null ||
                          !x.GeographyObjectCoordinates.Any(y => y.DateDeleted == null));
            if (typeId.HasValue) query = query.Where(x => x.TypeId == typeId);
            if (typeIds?.Length > 0) query = query.Where(x => typeIds.Contains(x.TypeId));

            //Получение данных из бд
            List<GeographyObject> data = await query.ToListAsync();

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
    /// Метод восстановления географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<bool> Restore(long? id, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredRestoreGeographyObjectMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Получение данных из бд
            GeographyObject data = await GetById(id) ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Проверки
            if (data.DateDeleted == null) throw new Exception(ErrorMessagesGeography.NotDeletedGeographyObject);

            //Запись данных в бд
            data.SetRestored();
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

    /// <summary>
    /// Метод закрытия географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <returns cref="bool">Признак успешности</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<bool> Close(long? id, string username)
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredCloseGeographyObjectMethod);

            //Проверки
            if (id == null) throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Получение данных из бд
            GeographyObject data = await GetById(id) ?? throw new Exception(ErrorMessagesGeography.NotFoundGeographyObject);

            //Проверки
            if (data.DateDeleted != null) throw new Exception(ErrorMessagesGeography.DeletedGeographyObject);

            //Запись данных в бд
            data.SetDeleted();
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