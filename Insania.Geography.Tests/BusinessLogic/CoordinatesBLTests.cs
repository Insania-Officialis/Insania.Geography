using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using NetTopologySuite.Geometries;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Models.Requests.Coordinates;
using Insania.Geography.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.Tests.BusinessLogic;

/// <summary>
/// Тесты сервиса работы с бизнес-логикой координат
/// </summary>
[TestFixture]
public class CoordinatesBLTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными координат
    /// </summary>
    private ICoordinatesDAO CoordinatesDAO { get; set; }

    /// <summary>
    /// Сервис работы с бизнес-логикой координат
    /// </summary>
    private ICoordinatesBL CoordinatesBL { get; set; }

    /// <summary>
    /// Сервис преобразования полигона
    /// </summary>
    private IPolygonParserSL PolygonParserSL { get; set; }

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
        CoordinatesDAO = ServiceProvider.GetRequiredService<ICoordinatesDAO>();
        CoordinatesBL = ServiceProvider.GetRequiredService<ICoordinatesBL>();
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
    /// Тест метода изменения координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    /// <param cref="Polygon?" name="coordinates">Координаты</param>
    [TestCase(null, null)]
    [TestCase(-1, null)]
    [TestCase(-1, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]")]
    [TestCase(1, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]")]
    [TestCase(2, "[[[0, 0],[0, 5],[5, 0],[0, 0]]]")]
    [TestCase(2, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]")]
    public async Task EditTest(long? id, string? coordinates)
    {
        try
        {
            //Получение значения до
            CoordinateGeography? coordinateBefore = null;
            if (id != null)
            {
                Shared.Entities.Coordinate? coordinate = await CoordinatesDAO.GetById(id);
                if (coordinate != null) coordinateBefore = new(coordinate);
            }

            //Формирование запроса
            double[][][]? coordinatesArray = null;
            Polygon? polygon = null;
            if (!string.IsNullOrWhiteSpace(coordinates))
            {
                coordinatesArray = JsonSerializer.Deserialize<double[][][]>(coordinates);
                if (coordinatesArray != null) polygon = PolygonParserSL.FromDoubleArrayToPolygon(coordinatesArray);
            }
            CoordinateEditRequest? request = id == null && polygon == null ? null : new(id, coordinatesArray);

            //Получение результата
            BaseResponse? result = await CoordinatesBL.Edit(request, _username);

            //Получение значения после
            CoordinateGeography? coordinateAfter = null;
            if (id != null) coordinateAfter = await CoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id, coordinates)
            {
                case (2, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]"):
                    Assert.That(result.Success, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateCreate, Is.LessThan(coordinateAfter!.DateUpdate));
                    Assert.That(coordinateBefore!.PolygonEntity, Is.Not.EqualTo(coordinateAfter!.PolygonEntity));
                    Assert.That(coordinateAfter!.PolygonEntity, Is.EqualTo(polygon));
                    await CoordinatesDAO.Edit(id, coordinateBefore.PolygonEntity, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id, coordinates)
            {
                case (null, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.EmptyRequest)); break;
                case (-1, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); break;
                case (-1, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.EmptyCoordinates)); break;
                case (1, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedCoordinate)); break;
                case (2, "[[[0, 0],[0, 5],[5, 0],[0, 0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotChangesCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}