using AutoMapper;
using BlossomApi.Dtos;
using BlossomApi.Dtos.Banners;
using BlossomApi.Dtos.Product;
using BlossomApi.Models;

namespace BlossomApi.Mappers
{
    public class BannerProfile : Profile
    {
        public BannerProfile()
        {
            // Mapping from Banner to BannerResponseDto
            CreateMap<Banner, BannerResponseDto>()
                .ForMember(dest => dest.BannerId, opt => opt.MapFrom(src => src.BannerId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DesktopImageUrl, opt => opt.MapFrom(src => src.DesktopImageUrl))
                .ForMember(dest => dest.LaptopImageUrl, opt => opt.MapFrom(src => src.LaptopImageUrl))
                .ForMember(dest => dest.TabletImageUrl, opt => opt.MapFrom(src => src.TabletImageUrl))
                .ForMember(dest => dest.PhoneImageUrl, opt => opt.MapFrom(src => src.PhoneImageUrl))
                .ForMember(dest => dest.DesktopAltText, opt => opt.MapFrom(src => src.DesktopAltText))
                .ForMember(dest => dest.LaptopAltText, opt => opt.MapFrom(src => src.LaptopAltText))
                .ForMember(dest => dest.TabletAltText, opt => opt.MapFrom(src => src.TabletAltText))
                .ForMember(dest => dest.PhoneAltText, opt => opt.MapFrom(src => src.PhoneAltText));

            // Mapping from Banner to BannerWithProductsDto
            CreateMap<Banner, BannerWithProductsDto>()
                .IncludeBase<Banner, BannerResponseDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            // Mapping from Product to ProductCardDto
            CreateMap<Product, ProductCardDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.FirstOrDefault())) // Assuming the first image is the main one
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.EngName, opt => opt.MapFrom(src => src.NameEng))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.IsHit, opt => opt.MapFrom(src => src.IsHit))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.InStock))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));
        }
    }
}
