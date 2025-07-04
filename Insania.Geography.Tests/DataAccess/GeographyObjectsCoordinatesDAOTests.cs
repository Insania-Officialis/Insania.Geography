using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;
using Microsoft.Extensions.DependencyInjection;
using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;
using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

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
    [TestCase(null)]
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
        catch (Exception ex)
        {
            //Проверка исключения
            switch (geographyObjectId)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
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
            GeographyObjectCoordinate? coordinateBefore = null;
            if (id != null)
            {
                coordinateBefore = await GeographyObjectsCoordinatesDAO.GetById(id);
            }

            //Получение результата
            bool? result = await GeographyObjectsCoordinatesDAO.Restore(id, _username);

            //Получение значения после
            GeographyObjectCoordinate? coordinateAfter = null;
            if (id != null) coordinateAfter = await GeographyObjectsCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 1:
                    Assert.That(result, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateCreate, Is.LessThan(coordinateAfter!.DateUpdate));
                    Assert.That(coordinateAfter!.DateDeleted, Is.Null);
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
            GeographyObjectCoordinate? coordinateBefore = null;
            if (id != null)
            {
                coordinateBefore = await GeographyObjectsCoordinatesDAO.GetById(id);
            }

            //Получение результата
            bool? result = await GeographyObjectsCoordinatesDAO.Close(id, _username);

            //Получение значения после
            GeographyObjectCoordinate? coordinateAfter = null;
            if (id != null) coordinateAfter = await GeographyObjectsCoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 2:
                    Assert.That(result, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateCreate, Is.LessThan(coordinateAfter!.DateUpdate));
                    Assert.That(coordinateAfter!.DateDeleted, Is.Not.Null);
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