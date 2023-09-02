using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class RockMappings : Profile
    {
        public RockMappings()
        {
            CreateMap<RockEntity, RockDto>().ReverseMap();
            CreateMap<RockEntity, RockUpdateDto>().ReverseMap();
            CreateMap<RockEntity, RockCreateDto>().ReverseMap();
        }
    }
}
