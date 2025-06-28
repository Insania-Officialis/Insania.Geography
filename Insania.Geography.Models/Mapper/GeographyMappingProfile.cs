using AutoMapper;

using Insania.Shared.Models.Responses.Base;

using Insania.Geography.Entities;

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
        //Преобразование модели сущности страны в базовую модель элемента ответа списком
        //CreateMap<Country, BaseResponseListItem>();
    }
}