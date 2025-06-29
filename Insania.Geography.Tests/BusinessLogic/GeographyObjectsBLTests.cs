using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Tests.Base;

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
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Items, Is.Not.Null);
                Assert.That(result.Items, Is.Not.Empty);
            });
        }
        catch (Exception)
        {
            //Проброс исключения
            throw;
        }
    }
    #endregion
}