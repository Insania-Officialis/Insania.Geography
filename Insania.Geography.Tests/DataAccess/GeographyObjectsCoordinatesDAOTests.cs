using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными координат географических объектов
/// </summary>
[TestFixture]
public class GeographyObjectsCoordinatesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Логин пользователя, выполняющего действие
    /// </summary>
    private readonly string _username = "test";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис работы с данными координат географических объектов
    /// </summary>
    private IGeographyObjectsCoordinatesDAO GeographyObjectsCoordinatesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными географических объектов
    /// </summary>
    private IGeographyObjectsDAO GeographyObjectsDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными координат 
    /// </summary>
    private ICoordinatesDAO CoordinatesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        GeographyObjectsCoordinatesDAO = ServiceProvider.GetRequiredService<IGeographyObjectsCoordinatesDAO>();
        GeographyObjectsDAO = ServiceProvider.GetRequiredService<IGeographyObjectsDAO>();
        CoordinatesDAO = ServiceProvider.GetRequiredService<ICoordinatesDAO>();
    }

    /// <summary>
    /// Метод, вызываемый после тестов
    /// </summary>
    [TearDown]
    public void TearDown()
    {

    }
    #endregion

    #region Методы тестирования
    /// <summary>
    /// Тест метода получения координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task GetByIdTest(long? id)
    {
        try
        {
            //Получение результата
            GeographyObjectCoordinate? result = await GeographyObjectsCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case -1: Assert.That(result, Is.Null); break;
                case 1: case 2: Assert.That(result, Is.Not.Null); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObjectCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода получения координаты географического объекта по идентификаторам географического объекта и координаты
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    [TestCase(null, null)]
    [TestCase(-1, null)]
    [TestCase(-1, -1)]
    [TestCase(1, 1)]
    [TestCase(1, 2)]
    public async Task GetByGeographyObjectIdAndCoordinateIdTest(long? geographyObjectId, long? coordinateId)
    {
        try
        {
            //Получение результата
            GeographyObjectCoordinate? result = await GeographyObjectsCoordinatesDAO.GetByGeographyObjectIdAndCoordinateId(geographyObjectId, coordinateId);

            //Проверка результата
            switch (geographyObjectId, coordinateId)
            {
                case (-1, -1): Assert.That(result, Is.Null); break;
                case (1, 2): case (1, 1): Assert.That(result, Is.Not.Null); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (geographyObjectId, coordinateId)
            {
                case (null, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
                case (-1, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода получения списка координат географических объектов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<GeographyObjectCoordinate>? result = await GeographyObjectsCoordinatesDAO.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Тест метода получения списка координат географических объектов по идентификатору географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    [TestCase(-1)]
    [TestCase(10000)]
    [TestCase(1)]
    public async Task GetListTest(long? geographyObjectId)
    {
        try
        {
            //Получение результата
            List<GeographyObjectCoordinate>? result = await GeographyObjectsCoordinatesDAO.GetList(geographyObjectId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (geographyObjectId)
            {
                case -1: case 10000: Assert.That(result, Is.Empty); break;
                case 1: Assert.That(result, Is.Not.Empty); Assert.That(!result.Any(x => x.GeographyObjectEntity == null)); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }

    /// <summary>
    /// Тест метода добавления координаты
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <param cref="int?" name="zoom">Коэффициент масштаба отображения сущности</param>
    [TestCase(null, null, null)]
    [TestCase(-1, null, null)]
    [TestCase(1, null, null)]
    [TestCase(1, -1, null)]
    [TestCase(1, 1, null)]
    [TestCase(1, 1, -1)]
    [TestCase(10000, 1, -1)]
    [TestCase(1, 1, -1)]
    [TestCase(1, 3, -1)]
    [TestCase(1, 3, 3)]
    public async Task AddTest(long? geographyObjectId, long? coordinateId, int? zoom)
    {
        try
        {
            //Формирование запроса
            GeographyObject? geographyObject = null;
            if (geographyObjectId != null) geographyObject = await GeographyObjectsDAO.GetById(geographyObjectId);
            CoordinateGeography? coordinate = null;
            if (coordinateId != null) coordinate = await CoordinatesDAO.GetById(coordinateId);

            //Получение результата
            long? result = await GeographyObjectsCoordinatesDAO.Add(geographyObject, coordinate, zoom, _username);

            //Получение значения
            GeographyObjectCoordinate? geographyObjectCoordinate = null;
            if (result != null) geographyObjectCoordinate = await GeographyObjectsCoordinatesDAO.GetById(result);

            //Проверка результата
            switch (geographyObjectId, coordinateId, zoom)
            {
                case (1, 3, 3):
                    Assert.That(result, Is.Positive);
                    Assert.That(geographyObjectCoordinate, Is.Not.Null);
                    Assert.That(geographyObjectCoordinate?.DateDeleted, Is.Null);
                    Assert.That(geographyObjectCoordinate?.GeographyObjectId, Is.EqualTo(geographyObjectId));
                    Assert.That(geographyObjectCoordinate?.CoordinateId, Is.EqualTo(coordinateId));
                    await GeographyObjectsCoordinatesDAO.Close(result, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (geographyObjectId, coordinateId, zoom)
            {
                case (null, null, null): case (-1, null, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
                case (1, null, null): case (1, -1, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); break;
                case (1, 1, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.EmptyZoom)); break;
                case (10000, 1, -1): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedGeographyObject)); break;
                case (1, 1, -1): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedCoordinate)); break;
                case (1, 3, -1): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.IncorrectZoom)); break;
                case (1, 2, 1): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.ExistsGeographyObjectCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода восстановления координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task RestoreTest(long? id)
    {
        try
        {
            //Получение значения до
            GeographyObjectCoordinate? geographyObjectCoordinateBefore = null;
            if (id != null)
            {
                geographyObjectCoordinateBefore = await GeographyObjectsCoordinatesDAO.GetById(id);
            }

            //Получение результата
            bool? result = await GeographyObjectsCoordinatesDAO.Restore(id, _username);

            //Получение значения после
            GeographyObjectCoordinate? geographyObjectCoordinateAfter = null;
            if (id != null) geographyObjectCoordinateAfter = await GeographyObjectsCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 1:
                    Assert.That(result, Is.True);
                    Assert.That(geographyObjectCoordinateBefore, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateAfter, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateBefore!.Id, Is.EqualTo(geographyObjectCoordinateAfter!.Id));
                    Assert.That(geographyObjectCoordinateBefore!.DateCreate, Is.LessThan(geographyObjectCoordinateAfter!.DateUpdate));
                    Assert.That(geographyObjectCoordinateAfter!.DateDeleted, Is.Null);
                    await GeographyObjectsCoordinatesDAO.Close(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObjectCoordinate)); break;
                case 2: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotDeletedGeographyObjectCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода закрытия координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task CloseTest(long? id)
    {
        try
        {
            //Получение значения до
            GeographyObjectCoordinate? geographyObjectCoordinateBefore = null;
            if (id != null)
            {
                geographyObjectCoordinateBefore = await GeographyObjectsCoordinatesDAO.GetById(id);
            }

            //Получение результата
            bool? result = await GeographyObjectsCoordinatesDAO.Close(id, _username);

            //Получение значения после
            GeographyObjectCoordinate? geographyObjectCoordinateAfter = null;
            if (id != null) geographyObjectCoordinateAfter = await GeographyObjectsCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 2:
                    Assert.That(result, Is.True);
                    Assert.That(geographyObjectCoordinateBefore, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateAfter, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateBefore!.Id, Is.EqualTo(geographyObjectCoordinateAfter!.Id));
                    Assert.That(geographyObjectCoordinateBefore!.DateCreate, Is.LessThan(geographyObjectCoordinateAfter!.DateUpdate));
                    Assert.That(geographyObjectCoordinateAfter!.DateDeleted, Is.Not.Null);
                    await GeographyObjectsCoordinatesDAO.Restore(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObjectCoordinate)); break;
                case 1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedGeographyObjectCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}