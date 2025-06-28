using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными координат географических объектов
/// </summary>
[TestFixture]
public class GeographyObjectsCoordinatesDAOTests : BaseTest
{
    #region Поля
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
    #endregion
}