using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using NetTopologySuite.Geometries;

using Insania.Shared.Contracts.Services;

using Insania.Geography.Contracts.DataAccess;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными координат
/// </summary>
[TestFixture]
public class CoordinatesDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными координат
    /// </summary>
    private ICoordinatesDAO CoordinatesDAO { get; set; }

    /// <summary>
    /// Сервис работы с данными типов координат
    /// </summary>
    private ICoordinatesTypesDAO CoordinatesTypesDAO { get; set; }

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
        CoordinatesTypesDAO = ServiceProvider.GetRequiredService<ICoordinatesTypesDAO>();
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
    /// Тест метода получения координаты по идентификатору
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task GetByIdTest(long? id)
    {
        try
        {
            //Получение результата
            CoordinateGeography? result = await CoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case -1: Assert.That(result, Is.Null); break;
                case 1: case 2: Assert.That(result, Is.Not.Null); break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода получения списка координат
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<CoordinateGeography>? result = await CoordinatesDAO.GetList();

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

    /// <summary>
    /// Тест метода изменения координаты
    /// </summary>
    /// <param cref="string?" name="coordinates">Координаты</param>
    /// <param cref="long?" name="typeId">Идентификатор типа координаты</param>
    [TestCase(null, null)]
    [TestCase("[[[0, 0],[0, 5],[5, 0]]]", null)]
    [TestCase("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]", null)]
    [TestCase("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]", -1)]
    [TestCase("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]", 1)]
    [TestCase("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]", 2)]
    public async Task AddTest(string? coordinates, long? typeId)
    {
        try
        {
            //Формирование запроса
            Polygon? polygon = null;
            if (!string.IsNullOrWhiteSpace(coordinates))
            {
                double[][][]? coordinatesArray = JsonSerializer.Deserialize<double[][][]>(coordinates);
                if (coordinatesArray != null)
                {
                    polygon = PolygonParserSL.FromDoubleArrayToPolygon(coordinatesArray) ?? throw new Exception(ErrorMessagesShared.IncorrectCoordinates);
                }
            }
            CoordinateTypeGeography? type = null;
            if (typeId != null) type = await CoordinatesTypesDAO.GetById(typeId);

            //Получение результата
            long? result = await CoordinatesDAO.Add(polygon, type, _username);

            //Получение значения
            CoordinateGeography? coordinate = await CoordinatesDAO.GetById(result);

            //Проверка результата
            switch (coordinates, typeId)
            {
                case ("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]", 2):
                    Assert.That(result, Is.Positive);
                    Assert.That(coordinate, Is.Not.Null);
                    Assert.That(coordinate?.PolygonEntity, Is.Not.Null);
                    await CoordinatesDAO.Close(result, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (coordinates, typeId)
            {
                case (null, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.EmptyCoordinates)); break;
                case ("[[[0, 0],[0, 5],[5, 0]]]", null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.IncorrectCoordinates)); break;
                case ("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]",null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinateType)); break;
                case ("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]", -1): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinateType)); break;
                case ("[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]", 1): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedCoordinateType)); break;
                default: throw;
            }
        }
    }

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
            Polygon? polygon = null;
            if (!string.IsNullOrWhiteSpace(coordinates))
            {
                double[][][]? coordinatesArray = JsonSerializer.Deserialize<double[][][]>(coordinates);
                if (coordinatesArray != null)
                {
                    polygon = PolygonParserSL.FromDoubleArrayToPolygon(coordinatesArray);
                }
            }

            //Получение результата
            bool? result = await CoordinatesDAO.Edit(id, polygon, _username);

            //Получение значения после
            CoordinateGeography? coordinateAfter = null;
            if (id != null) coordinateAfter = await CoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id, coordinates)
            {
                case (2, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]"): 
                    Assert.That(result, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateUpdate, Is.LessThan(coordinateAfter!.DateUpdate));
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
                case (null, null):
                case (-1, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]"):
                    Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); 
                    break;
                case (-1, null): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesShared.EmptyCoordinates)); break;
                case (1, "[[[0, 0],[0, 20],[20, 20],[20, 0],[0, 0]],[[5, 5],[5, 15],[15, 15],[15, 5],[5, 5]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedCoordinate)); break;
                case (2, "[[[0, 0],[0, 5],[5, 0],[0, 0]]]"): Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotChangesCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода восстановления координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task RestoreTest(long? id)
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

            //Получение результата
            bool? result = await CoordinatesDAO.Restore(id, _username);

            //Получение значения после
            CoordinateGeography? coordinateAfter = null;
            if (id != null) coordinateAfter = await CoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 1:
                    Assert.That(result, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateCreate, Is.LessThan(coordinateAfter!.DateUpdate));
                    Assert.That(coordinateAfter!.DateDeleted, Is.Null);
                    await CoordinatesDAO.Close(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); break;
                case 2: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotDeletedCoordinate)); break;
                default: throw;
            }
        }
    }

    /// <summary>
    /// Тест метода закрытия координаты
    /// </summary>
    /// <param cref="long?" name="id">Идентификатор координаты</param>
    [TestCase(null)]
    [TestCase(-1)]
    [TestCase(1)]
    [TestCase(2)]
    public async Task CloseTest(long? id)
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

            //Получение результата
            bool? result = await CoordinatesDAO.Close(id, _username);

            //Получение значения после
            CoordinateGeography? coordinateAfter = null;
            if (id != null) coordinateAfter = await CoordinatesDAO.GetById(id);

            //Проверка результата
            switch (id)
            {
                case 2:
                    Assert.That(result, Is.True);
                    Assert.That(coordinateBefore, Is.Not.Null);
                    Assert.That(coordinateAfter, Is.Not.Null);
                    Assert.That(coordinateBefore!.Id, Is.EqualTo(coordinateAfter!.Id));
                    Assert.That(coordinateBefore!.DateCreate, Is.LessThan(coordinateAfter!.DateUpdate));
                    Assert.That(coordinateAfter!.DateDeleted, Is.Not.Null);
                    await CoordinatesDAO.Restore(id, _username);
                    break;
                default: throw new Exception(ErrorMessagesShared.NotFoundTestCase);
            }
        }
        catch (Exception ex)
        {
            //Проверка исключения
            switch (id)
            {
                case null: case -1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.NotFoundCoordinate)); break;
                case 1: Assert.That(ex.Message, Is.EqualTo(ErrorMessagesGeography.DeletedCoordinate)); break;
                default: throw;
            }
        }
    }
    #endregion
}