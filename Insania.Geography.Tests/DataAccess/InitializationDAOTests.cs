using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.DataAccess;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса инициализации данных в бд географии
/// </summary>
[TestFixture]
public class InitializationDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис инициализации данных в бд географии
    /// </summary>
    private IInitializationDAO InitializationDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными типов географических объектов
    /// </summary>
    private IGeographyObjectsTypesDAO GeographyObjectsTypesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными географических объектов
    /// </summary>
    private IGeographyObjectsDAO GeographyObjectsDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными типов координат
    /// </summary>
    private ICoordinatesTypesDAO CoordinatesTypesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными координат
    /// </summary>
    private ICoordinatesDAO CoordinatesDAO { get; set; }

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
        InitializationDAO = ServiceProvider.GetRequiredService<IInitializationDAO>();
        GeographyObjectsTypesDAO = ServiceProvider.GetRequiredService<IGeographyObjectsTypesDAO>();
        GeographyObjectsDAO = ServiceProvider.GetRequiredService<IGeographyObjectsDAO>();
        CoordinatesTypesDAO = ServiceProvider.GetRequiredService<ICoordinatesTypesDAO>();
        CoordinatesDAO = ServiceProvider.GetRequiredService<ICoordinatesDAO>();
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
    /// Тест метода инициализации данных
    /// </summary>
    [Test]
    public async Task InitializeTest()
    {
        try
        {
            //Выполнение метода
            await InitializationDAO.Initialize();

            //Получение сущностей
            List<GeographyObjectType> geographyObjectsTypes = await GeographyObjectsTypesDAO.GetList();
            List<GeographyObject> geographyObjects = await GeographyObjectsDAO.GetList();
            List<CoordinateTypeGeography> coordinateTypes = await CoordinatesTypesDAO.GetList();
            List<CoordinateGeography> coordinates = await CoordinatesDAO.GetList();
            List<GeographyObjectCoordinate> geographyObjectsCoordinates = await GeographyObjectsCoordinatesDAO.GetList();
            
            //Проверка результата
            using (Assert.EnterMultipleScope())
            {
                Assert.That(geographyObjectsTypes, Is.Not.Empty);
                Assert.That(geographyObjects, Is.Not.Empty);
                Assert.That(coordinateTypes, Is.Not.Empty);
                Assert.That(coordinates, Is.Not.Empty);
                Assert.That(geographyObjectsCoordinates, Is.Not.Empty);
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