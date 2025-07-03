using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using Insania.Shared.Contracts.Services;
using Insania.Shared.Entities;

namespace Insania.Geography.Entities;

/// <summary>
/// Модель сущности географического объекта
/// </summary>
[Table("c_geography_objects")]
[Comment("Географические объекты")]
public class GeographyObject : Compendium
{
    #region Конструкторы
    /// <summary>
    /// Простой конструктор модели сущности типа географического объекта
    /// </summary>
    public GeographyObject() : base()
    {
        TypeEntity = new();
    }

    /// <summary>
    /// Конструктор модели сущности типа географического объекта без идентификатора
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="GeographyObjectType" name="type">Тип</param>
    /// <param cref="GeographyObject?" name="parent">Родитель</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public GeographyObject(ITransliterationSL transliteration, string username, string name, GeographyObjectType type, GeographyObject? parent = null, DateTime? dateDeleted = null) : base(transliteration, username, name, dateDeleted)
    {
        TypeId = type.Id;
        TypeEntity = type;
        ParentId = parent?.Id;
        ParentEntity = parent;
    }

    /// <summary>
    /// Конструктор модели сущности типа географического объекта с идентификатором
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="long?" name="id">Идентификатор пользователя</param>
    /// <param cref="string" name="username">Логин пользователя, выполняющего действие</param>
    /// <param cref="string" name="name">Наименование</param>
    /// <param cref="GeographyObjectType" name="type">Тип</param>
    /// <param cref="GeographyObject?" name="parent">Родитель</param>
    /// <param cref="DateTime?" name="dateDeleted">Дата удаления</param>
    public GeographyObject(ITransliterationSL transliteration, long id, string username, string name, GeographyObjectType type, GeographyObject? parent = null, DateTime? dateDeleted = null) : base(transliteration, id, username, name, dateDeleted)
    {
        TypeId = type.Id;
        TypeEntity = type;
        ParentId = parent?.Id;
        ParentEntity = parent;
    }

    /// <summary>
    /// Конструктор модели сущности географического объекта с сущностью
    /// </summary>
    /// <param cref="ITransliterationSL" name="transliteration">Сервис транслитерации</param>
    /// <param cref="GeographyObject" name="entity">Базовая сущность</param>
    public GeographyObject(ITransliterationSL transliteration, GeographyObject entity) : base(transliteration, entity.Id, entity.UsernameCreate, entity.Name, entity.DateDeleted)
    {
        TypeId = entity.TypeId;
        TypeEntity = entity.TypeEntity;
        ParentId = entity.ParentId;
        ParentEntity = entity.ParentEntity;
    }
    #endregion

    #region Поля
    /// <summary>
    ///	Идентификатор типа
    /// </summary>
    [Column("type_id")]
    [Comment("Идентификатор типа")]
    [ForeignKey(nameof(TypeEntity))]
    public long TypeId { get; private set; }

    /// <summary>
    ///	Идентификатор родителя
    /// </summary>
    [Column("parent_id")]
    [Comment("Идентификатор родителя")]
    [ForeignKey(nameof(ParentEntity))]
    public long? ParentId { get; private set; }
    #endregion

    #region Навигационные свойства
    /// <summary>
    /// Навигационное свойство типа
    /// </summary>
    public GeographyObjectType TypeEntity { get; private set; }

    /// <summary>
    /// Навигационное свойство родителя
    /// </summary>
    public GeographyObject? ParentEntity { get; private set; }
    #endregion

    #region Методы
    /// <summary>
    /// Метод записи типа
    /// </summary>
    /// <param cref="GeographyObjectType" name="type">Тип</param>
    public void SetType(GeographyObjectType type)
    {
        TypeId = type.Id;
        TypeEntity = type;
    }

    /// <summary>
    /// Метод записи родителя
    /// </summary>
    /// <param cref="GeographyObject?" name="parent">Родитель</param>
    public void SetParent(GeographyObject? parent)
    {
        ParentId = parent?.Id;
        ParentEntity = parent;
    }
    #endregion
}