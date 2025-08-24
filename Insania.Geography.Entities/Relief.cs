using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Geography.Entities;

/// <summary>
/// Модель сущности рельефа
/// </summary>
[Table("c_relief")]
[Comment("Рельефы")]
public class Relief : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности типа рельефа
    /// </summary>
    public Relief() : base()
    {
        Color = "";
    }

    /// <summary>
    /// Конструктор модели сущности типа рельефа без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="int" name="level">Уровень</param>
    /// <param cref="string" name="color">Цвет на карте</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Relief(ITransliterationSL transliteration, string username, string name, int level, string color, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        Level = level;
        Color = color;
    }

    /// <summary>
    /// Конструктор модели сущности типа рельефа с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="ReliefType" name="type">Тип</param>
    /// <param cref="Relief?" name="parent">Родитель</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public Relief(ITransliterationSL transliteration, long id, string username, string name, int level, string color, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        Level = level;
        Color = color;
    }

    /// <summary>
    /// Конструктор модели сущности рельефа с сущностью
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="Relief" name="entity">Базовая сущность</param>
    public Relief(ITransliterationSL transliteration, Relief entity) : base(transliteration, entity.Id, entity.UsernameCreate, entity.Name, entity.DateDeleted)
    {
        Level = entity.Level;
        Color = entity.Color;
    }
    #endregion

    #region Поля
    /// <summary>
    ///	Уровень
    /// </summary>
    [Column("level")]
    [Comment("Уровень")]
    public int Level { get; private set; }

    /// <summary>
    ///	Цвет на карте
    /// </summary>
    [Column("color")]
    [Comment("Цвет на карте")]
    public string Color { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи уровня
    /// </summary>
    /// <param cref="int" name="level">Уровень</param>
    public void SetLevel(int level) => Level = level;

    /// <summary>
    /// Метод записи цвета на карте
    /// </summary>
    /// <param cref="string" name="color">Цвет на карте</param>
    public void SetColor(string color) => Color = color;
    #endregion
}