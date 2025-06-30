using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;
using Insania.Geography.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой координат географических объектов
/// </summary>
[TestFixture]
public class GeographyObjectsCoordinatesBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с бизнес-логикой координат географических объектов
    /// </summary>
    private IGeographyObjectsCoordinatesBL GeographyObjectsCoordinatesBL { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        GeographyObjectsCoordinatesBL = ServiceProvider.GetRequiredService<IGeographyObjectsCoordinatesBL>();
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
            GeographyObjectsCoordinatesResponseList? result = await GeographyObjectsCoordinatesBL.GetList(geographyObjectId);

            //Проверка результата
            Assert.That(result, Is.Not.Null);
            switch (geographyObjectId)
            {
                case 1:
                    Assert.That(string.IsNullOrWhiteSpace(result.Name), Is.False);
                    Assert.That(result.Center, Is.Not.Null);
                    Assert.That(result.Zoom, Is.Positive);
                    Assert.That(result.Items, Is.Not.Empty);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (geographyObjectId)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
                case -1: case 10000: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObjectCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}