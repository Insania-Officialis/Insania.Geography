using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Entities;
using Insania.Geography.Models.Responses.GeographyObjectsCoordinates;

namespace Insania.Geography.Models.Mapper;

/// <summary>
/// Сервис преобразования моделей
/// </summary>
public class GeographyMappingProfile : Profile
{
    /// <summary>
    /// Конструктор сервиса преобразования моделей
    /// </summary>
    public GeographyMappingProfile()
    {
        //Преобразование модели сущности географического объекта в базовую модель элемента ответа списком
        CreateMap<GeographyObject, BaseResponseListItem>();

        //Преобразование модели сущности координаты в модель элемента ответа списком координат географических объектов
        CreateMap<CoordinateGeography, GeographyObjectsCoordinatesResponseListItem>().ForMember(x => x.Coordinates, y => y.MapFrom(z => z.PolygonEntity));
    }
}