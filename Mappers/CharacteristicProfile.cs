using AutoMapper;
using BlossomApi.Dtos.Characteristic;
using BlossomApi.Models;

namespace BlossomApi.Mappers
{
    public class CharacteristicProfile : Profile
    {
        public CharacteristicProfile()
        {
            CreateMap<Characteristic, CharacteristicDto>();
            CreateMap<CharacteristicCreateDto, Characteristic>();
            CreateMap<CharacteristicUpdateDto, Characteristic>();
        }
    }
}