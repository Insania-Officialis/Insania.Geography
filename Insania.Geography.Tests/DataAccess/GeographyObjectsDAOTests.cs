using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

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
    #endregion
}