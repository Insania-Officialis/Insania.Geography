using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;
using Insania.Shared.Contracts.Services;
using Insania.Geography.Entities;
using Insania.Geography.Tests.Base;

using ErrorMessagesShared = Insania.Shared.Messages.ErrorMessages;

using ErrorMessagesGeography = Insania.Geography.Messages.ErrorMessages;

namespace Insania.Geography.Tests.DataAccess;

/// <summary>
/// Тесты сервиса работы с данными рельефов
/// </summary>
[TestFixture]
public class ReliefsDAOTests : BaseTest
{
    #region Поля
    /// <summary>
    /// Сервис работы с данными рельефов
    /// </summary>
    private IReliefsDAO ReliefsDAO { get; set; }

    /// <summary>
    /// Сервис транслитерации
    /// </summary>
    private ITransliterationSL TransliterationSL { get; set; }

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
        ReliefsDAO = ServiceProvider.GetRequiredService<IReliefsDAO>();
        TransliterationSL = ServiceProvider.GetRequiredService<ITransliterationSL>();
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
    /// Тест метода получения списка рельефов
    /// </summary>
    [Test]
    public async Task GetListTest()
    {
        try
        {
            //Получение результата
            List<Relief>? result = await ReliefsDAO.GetList();

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