using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

namespace Insania.Geography.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой географических объектов
/// </summary>
[TestFixture]
public class GeographyObjectsBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с бизнес-логикой географических объектов
    /// </summary>
    private IGeographyObjectsBL GeographyObjectsBL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        GeographyObjectsBL = ServiceProvider.GetRequiredService<IGeographyObjectsBL>();
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
    /// Тест метода получения списка географических объектов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            BaseResponseList? result = await GeographyObjectsBL.GetList();

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Items, Is.Not.Null);
                Assert.That(result.Items, Is.Not.Empty);
            }
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
            BaseResponseList? result = await GeographyObjectsBL.GetList(hasCoordinates);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Items, Is.Not.Null);
                Assert.That(result.Items, Is.Not.Empty);
            }
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
            BaseResponseList? result = await GeographyObjectsBL.GetList(typeId: typeId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Items, Is.Not.Null);
            }
            switch (typeId)
            {
                case -1:
                case 1:
                    Assert.That(result.Items, Is.Empty);
                    break;
                case 4:
                    Assert.That(result.Items, Is.Not.Empty);
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
    #endregion
}