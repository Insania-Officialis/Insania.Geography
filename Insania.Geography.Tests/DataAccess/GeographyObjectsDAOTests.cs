using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными географических объектов
/// </summary>
[TestFixture]
public class GeographyObjectsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными географических объектов
    /// </summary>
    private IGeographyObjectsDAO GeographyObjectsDAO { get; set; }

    /// <summary>
    /// Сервис транслитерации
    /// </summary>
    private ITransliterationSL TransliterationSL { get; set; }

    /// <summary>
    /// Логин пользователя, выполняющего действие
    /// </summary>
    private readonly string _username = "test";
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        GeographyObjectsDAO = ServiceProvider.GetRequiredService<IGeographyObjectsDAO>();
        TransliterationSL = ServiceProvider.GetRequiredService<ITransliterationSL>();
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
    /// Тест метода получения географического объекта по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(10000)]
    [TestCase(1)]
    public async Task GetByIdTest(long? id)
    {
        try
        {
            //Получение результата
            GeographyObject? result = await GeographyObjectsDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case -1: Assert.That(result, Is.Null); break;
                case 10000: case 1: Assert.That(result, Is.Not.Null); Assert.That(result?.TypeEntity, Is.Not.Null); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода получения списка географических объектов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<GeographyObject>? result = await GeographyObjectsDAO.GetList();

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
    /// Тест метода получения списка географических объектов с проверкой наличия координат
    /// </summary>
    /// <param cref="bool?" name="hasCoordinates">Проверка наличия координат</param>
    [TestCase(false)]
    [TestCase(true)]
    public async Task GetListWithCheckCoordinatesTest(bool hasCoordinates)
    {
        try
        {
            //Получение результата
            List<GeographyObject>? result = await GeographyObjectsDAO.GetList(hasCoordinates: hasCoordinates);

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
    /// Тест метода получения списка географических объектов с проверкой наличия координат
    /// </summary>
    /// <param cref="long?" name="typeId">Идентификатор типа</param>
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(4)]
    public async Task GetListWithTypeTest(long typeId)
    {
        try
        {
            //Получение результата
            List<GeographyObject>? result = await GeographyObjectsDAO.GetList(typeId: typeId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (typeId)
            {
                case -1: case 1:
                    Assert.That(result, Is.Empty);
                    break;
                case 4:
                    Assert.That(result, Is.Not.Empty);
                    break;
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
    /// Тест метода восстановления географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(10000)]
    public async Task RestoreTest(long? id)
    {
        try
        {
            //Получение значения до
            GeographyObject? geographyObjectBefore = null;
            if (id != null)
            {
                GeographyObject? geographyObject = await GeographyObjectsDAO.GetById(id);
                if (geographyObject != null) geographyObjectBefore = new(TransliterationSL, geographyObject);
            }

            //Получение результата
            bool? result = await GeographyObjectsDAO.Restore(id, _username);

            //Получение значения после
            GeographyObject? geographyObjectAfter = null;
            if (id != null) geographyObjectAfter = await GeographyObjectsDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 10000:
                    Assert.That(result, Is.True);
                    Assert.That(geographyObjectBefore, Is.Not.Null);
                    Assert.That(geographyObjectAfter, Is.Not.Null);
                    Assert.That(geographyObjectBefore!.Id, Is.EqualTo(geographyObjectAfter!.Id));
                    Assert.That(geographyObjectBefore!.DateCreate, Is.LessThan(geographyObjectAfter!.DateUpdate));
                    Assert.That(geographyObjectAfter!.DateDeleted, Is.Null);
                    await GeographyObjectsDAO.Close(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
                case 1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotDeletedGeographyObject)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода закрытия географического объекта
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор географического объекта</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(10000)]
    [TestCase(1)]
    public async Task CloseTest(long? id)
    {
        try
        {
            //Получение значения до
            GeographyObject? geographyObjectBefore = null;
            if (id != null)
            {
                GeographyObject? geographyObject = await GeographyObjectsDAO.GetById(id);
                if (geographyObject != null) geographyObjectBefore = new(TransliterationSL, geographyObject);
            }

            //Получение результата
            bool? result = await GeographyObjectsDAO.Close(id, _username);

            //Получение значения после
            GeographyObject? geographyObjectAfter = null;
            if (id != null) geographyObjectAfter = await GeographyObjectsDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 1:
                    Assert.That(result, Is.True);
                    Assert.That(geographyObjectBefore, Is.Not.Null);
                    Assert.That(geographyObjectAfter, Is.Not.Null);
                    Assert.That(geographyObjectBefore!.Id, Is.EqualTo(geographyObjectAfter!.Id));
                    Assert.That(geographyObjectBefore!.DateCreate, Is.LessThan(geographyObjectAfter!.DateUpdate));
                    Assert.That(geographyObjectAfter!.DateDeleted, Is.Not.Null);
                    await GeographyObjectsDAO.Restore(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
                case 10000: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedGeographyObject)); break;
                default: throw;
            }
        }
    }
    #endregion
}