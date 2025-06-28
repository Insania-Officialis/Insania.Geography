using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.DataAccess;

namespace Insania.Geography.DataAccess;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с данными в зоне географии
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с данными в зоне географии
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddGeographyDAO(this IServiceCollection services) =>
        services
            .AddScoped<IGeographyObjectsTypesDAO, GeographyObjectsTypesDAO>() //сервис работы с данными типов географических объектов
            .AddScoped<IGeographyObjectsDAO, GeographyObjectsDAO>() //сервис работы с данными географических объектов
            .AddScoped<ICoordinatesTypesDAO, CoordinatesTypesDAO>() //сервис работы с данными типов координат
            .AddScoped<ICoordinatesDAO, CoordinatesDAO>() //сервис работы с данными координат
            .AddScoped<IGeographyObjectsCoordinatesDAO, GeographyObjectsCoordinatesDAO>() //сервис работы с данными координат географических объектов
        ;
}