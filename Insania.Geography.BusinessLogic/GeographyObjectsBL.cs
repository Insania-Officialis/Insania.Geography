using Microsoft.Extensions.Logging;

using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;

using ErrorMessages = Insania.Shared.Messages.ErrorMessages;
using InformationMessages = Insania.Geography.Messages.InformationMessages;

namespace Insania.Geography.BusinessLogic;

/// <summary>
/// Сервис работы с бизнес-логикой географических объектов
/// </summary>
/// <param cref="ILogger{GeographyObjectsBL}" name="logger">Сервис логгирования</param>
/// <param cref="IMapper" name="mapper">Сервис преобразования моделей</param>
/// <param cref="IGeographyObjectsDAO" name="geographyObjectsDAO">Сервис работы с данными географических объектов</param>
public class GeographyObjectsBL(ILogger<GeographyObjectsBL> logger, IMapper mapper, IGeographyObjectsDAO geographyObjectsDAO) : IGeographyObjectsBL
{
    #region Зависимости
    /// <summary>
    /// Сервис логгирования
    /// </summary>
    private readonly ILogger<GeographyObjectsBL> _logger = logger;

    /// <summary>
    /// Сервис преобразования моделей
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Сервис работы с данными географических объектов
    /// </summary>
    private readonly IGeographyObjectsDAO _geographyObjectsDAO = geographyObjectsDAO;
    #endregion

    #region Методы
    /// <summary>
    /// Метод получения списка географических объектов
    /// </summary>
    /// <returns cref="BaseResponseList">Стандартный ответ</returns>
    /// <exception cref="Exception">Исключение</exception>
    public async Task<BaseResponseList> GetList()
    {
        try
        {
            //Логгирование
            _logger.LogInformation(InformationMessages.EnteredGetListGeographyObjectsMethod);

            //Получение данных
            List<GeographyObject>? data = await _geographyObjectsDAO.GetList();

            //Формирование ответа
            BaseResponseList? response = null;
            if (data == null) response = new(false, null);
            else response = new(true, data?.Select(_mapper.Map<BaseResponseListItem>).ToList());

            //Возврат ответа
            return response;
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