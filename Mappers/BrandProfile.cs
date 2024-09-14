using AutoMapper;
using BlossomApi.Dtos.Brends;
using BlossomApi.Models;

namespace BlossomApi.Mappers
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            // Map from Brand to BrandResponseDto
            CreateMap<Brand, BrandResponseDto>();

            // Map from Brand to BrandWithProductsDto
            CreateMap<Brand, BrandWithProductsDto>();

            // Map from BrandCreateDto to Brand
            CreateMap<BrandCreateDto, Brand>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.LogoImageUrl, opt => opt.Ignore());

            // Map from BrandUpdateDto to Brand
            CreateMap<BrandUpdateDto, Brand>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.LogoImageUrl, opt => opt.Ignore());
        }
    }
}
