using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными типов координат
/// </summary>
[TestFixture]
public class CoordinatesTypesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными типов координат
    /// </summary>
    private ICoordinatesTypesDAO CoordinatesTypesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        CoordinatesTypesDAO = ServiceProvider.GetRequiredService<ICoordinatesTypesDAO>();
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
    /// Тест метода получения списка типов координат
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<CoordinateTypeGeography>? result = await CoordinatesTypesDAO.GetList();

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