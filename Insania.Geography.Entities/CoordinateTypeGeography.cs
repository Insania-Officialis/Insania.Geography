using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Geography.Entities;

/// <summary>
/// Модель сущности типа координаты географии
/// </summary>
[Table("c_coordinates_types")]
[Comment("Типы координат географии")]
public class CoordinateTypeGeography : CoordinateType
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности типа координаты географии
    /// </summary>
    public CoordinateTypeGeography() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа координаты географии без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="backgroundColor">Цвет фона</param>
    /// <param cref="string" name="borderColor">Цвет границ</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinateTypeGeography(ITransliterationSL transliteration, string username, string name, string backgroundColor, string borderColor, DateTime? dateDeleted = null) : base(transliteration, username, name, backgroundColor, borderColor, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа координаты географии с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long" name="id">Первичный ключ таблицы</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="string" name="backgroundColor">Цвет фона</param>
    /// <param cref="string" name="borderColor">Цвет границ</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public CoordinateTypeGeography(ITransliterationSL transliteration, long id, string username, string name, string backgroundColor, string borderColor, DateTime? dateDeleted = null) : base(transliteration, id, username, name, backgroundColor, borderColor, dateDeleted)
    {

    }
    #endregion
}