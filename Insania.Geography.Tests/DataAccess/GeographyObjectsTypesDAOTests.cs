using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными типов географических объектов
/// </summary>
[TestFixture]
public class GeographyObjectsTypesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными типов географических объектов
    /// </summary>
    private IGeographyObjectsTypesDAO GeographyObjectsTypesDAO { get; set; }
    #endregion

    #region Общие методы
    /// <summary>
    /// Метод, вызываемый до тестов
    /// </summary>
    [SetUp]
    public void Setup()
    {
        //Получение зависимости
        GeographyObjectsTypesDAO = ServiceProvider.GetRequiredService<IGeographyObjectsTypesDAO>();
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
    /// Тест метода получения списка типов географических объектов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<GeographyObjectType>? result = await GeographyObjectsTypesDAO.GetList();

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