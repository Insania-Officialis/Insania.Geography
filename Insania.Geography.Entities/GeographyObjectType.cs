using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Geography.Entities;

/// <summary>
/// Модель сущности типа географического объекта
/// </summary>
[Table("c_geography_objects_types")]
[Comment("Типы географических объектов")]
public class GeographyObjectType : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности типа географического объекта
    /// </summary>
    public GeographyObjectType() : base()
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа географического объекта без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public GeographyObjectType(ITransliterationSL transliteration, string username, string name, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {

    }

    /// <summary>
    /// Конструктор модели сущности типа географического объекта с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public GeographyObjectType(ITransliterationSL transliteration, long id, string username, string name, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {

    }
    #endregion
}