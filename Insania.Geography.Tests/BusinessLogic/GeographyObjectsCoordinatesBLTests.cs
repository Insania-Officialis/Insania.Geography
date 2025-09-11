using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Models.Requests.GeographyObjectsCoordinates;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;
using Insania.Geography.Entities;
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
    /// Логин пользователя, выполняющего действие
    /// </summary>
    private readonly string _username = "test";
    #endregion

    #region Зависимости
    /// <summary>
    /// Сервис работы с бизнес-логикой координат географических объектов
    /// </summary>
    private IGeographyObjectsCoordinatesBL GeographyObjectsCoordinatesBL { get; set; }

    /// <summary>
    /// Сервис работы с данными координат географических объектов
    /// </summary>
    private IGeographyObjectsCoordinatesDAO GeographyObjectsCoordinatesDAO { get; set; }

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private IPolygonParserSL PolygonParserSL { get; set; }
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
        GeographyObjectsCoordinatesDAO = ServiceProvider.GetRequiredService<IGeographyObjectsCoordinatesDAO>();
        PolygonParserSL = ServiceProvider.GetRequiredService<IPolygonParserSL>();
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
    public async Task GetByGeographyObjectIdTest(long? geographyObjectId)
    {
        try
        {
            //Получение результата
            GeographyObjectCoordinatesResponseList? result = await GeographyObjectsCoordinatesBL.GetByGeographyObjectId(geographyObjectId);

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

    /// <summary>
    /// Тест метода актуализации координаты географического объекта
    /// </summary>
    /// <param cref="long?" name="geographyObjectId">Идентификатор географического объекта</param>
    /// <param cref="long?" name="coordinateId">Идентификатор координаты</param>
    /// <param cref="string?" name="coordinates">Координаты</param>
    [TestCase(null, null, null)]
    [TestCase(-1, null, null)]
    [TestCase(-1, -1, null)]
    [TestCase(-1, -1, "[]")]
    [TestCase(-1, -1, "[[[0, 0],[0, 5],[5, 0]]]")]
    [TestCase(-1, -1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(10000, -1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(10000, 1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(1, 1, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(1, 3, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]")]
    [TestCase(10001, 3, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]")]
    [TestCase(4, 3, "[[[0,0],[0,5],[5,0],[0,0]]]")]
    [TestCase(1, 2, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]")]
    public async Task UpgradeTest(long? geographyObjectId, long? coordinateId, string? coordinates)
    {
        try
        {
            //Получение значения до
            GeographyObjectCoordinate? geographyObjectCoordinateBefore = null;
            if (geographyObjectId != null && coordinateId != null) geographyObjectCoordinateBefore = await GeographyObjectsCoordinatesDAO.GetByGeographyObjectIdAndCoordinateId(geographyObjectId, coordinateId);

            //Формирование запроса
            double[][][]? polygon = null;
            if (!string.IsNullOrWhiteSpace(coordinates))
            {
                polygon = JsonSerializer.Deserialize<double[][][]>(coordinates);
            }
            GeographyObjectsCoordinatesUpgradeRequest? request = new(geographyObjectId, coordinateId, polygon);

            //Получение результата
            BaseResponse result = await GeographyObjectsCoordinatesBL.Upgrade(request, _username);

            //Получение значения после
            GeographyObjectCoordinate? geographyObjectCoordinateAfter = await GeographyObjectsCoordinatesDAO.GetById(result?.Id);

            //Проверка результата
            switch (geographyObjectId, coordinateId, coordinates)
            {
                case (1, 2, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]"):
                    Assert.That(result, Is.Not.Null);
                    Assert.That(result?.Success, Is.True);
                    Assert.That(result?.Id, Is.Not.Null);
                    Assert.That(result?.Id, Is.Positive);
                    Assert.That(geographyObjectCoordinateBefore, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateBefore?.GeographyObjectEntity, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateBefore?.CoordinateEntity, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateAfter, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateAfter?.GeographyObjectEntity, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateAfter?.CoordinateEntity, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateBefore?.GeographyObjectEntity?.Id, Is.EqualTo(geographyObjectCoordinateAfter?.GeographyObjectEntity?.Id));
                    Assert.That(geographyObjectCoordinateBefore?.CoordinateEntity?.Id, Is.Not.EqualTo(geographyObjectCoordinateAfter?.CoordinateEntity?.Id));
                    Assert.That(geographyObjectCoordinateBefore?.CoordinateEntity?.PolygonEntity, Is.Not.EqualTo(geographyObjectCoordinateAfter?.CoordinateEntity?.PolygonEntity));
                    double[][][]? polygonAfter = PolygonParserSL.FromPolygonToDoubleArray(geographyObjectCoordinateAfter?.CoordinateEntity?.PolygonEntity);
                    Assert.That(polygonAfter, Is.EqualTo(polygon));
                    Assert.That(geographyObjectCoordinateBefore?.DateDeleted, Is.Not.Null);
                    Assert.That(geographyObjectCoordinateAfter?.DateDeleted, Is.Null);
                    double[][][]? polygonBefore = PolygonParserSL.FromPolygonToDoubleArray(geographyObjectCoordinateBefore?.CoordinateEntity?.PolygonEntity);
                    await GeographyObjectsCoordinatesDAO.Restore(geographyObjectCoordinateBefore?.Id, _username);
                    await GeographyObjectsCoordinatesDAO.Close(geographyObjectCoordinateAfter?.Id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (geographyObjectId, coordinateId, coordinates)
            {
                case (null, null, null): case (-1, -1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObject)); break;
                case (-1, null, null): case (10000, -1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); break;
                case (-1, -1, null): case (-1, -1, "[]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.EmptyCoordinates)); break;
                case (-1, -1, "[[[0, 0],[0, 5],[5, 0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.IncorrectCoordinates)); break;
                case (10000, 1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedGeographyObject)); break;
                case (1, 1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedCoordinate)); break;
                case (1, 3, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundGeographyObjectCoordinate)); break;
                case (10001, 3, "[[[0,0],[0,5],[5,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedGeographyObjectCoordinate)); break;
                case (2, 1, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.ExistsGeographyObjectCoordinate)); break;
                case (4, 3, "[[[0,0],[0,5],[5,0],[0,0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotChangesCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}