using Microsoft.Extensions.DependencyInjection;

using Insania.Geography.Contracts.BusinessLogic;
using Insania.Geography.DataAccess;

namespace Insania.Geography.BusinessLogic;

/// <summary>
/// Расширение для внедрения зависимостей сервисов работы с бизнес-логикой в зоне географии
/// </summary>
public static class Extension
{
    /// <summary>
    /// Метод внедрения зависимостей сервисов работы с бизнес-логикой в зоне географии
    /// </summary>
    /// <param cref="IServiceCollection" name="services">Исходная коллекция сервисов</param>
    /// <returns cref="IServiceCollection">Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddGeographyBL(this IServiceCollection services) =>
        services
            .AddGeographyDAO() //сервисы работы с данными в зоне географии
            .AddScoped<IGeographyObjectsBL, GeographyObjectsBL>() //сервис работы с бизнес-логикой географических объектов
        ;
}