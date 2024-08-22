using AutoMapper;
using BlossomApi.Dtos;
using BlossomApi.Models;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.AvailableAmount))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.MainCategory, opt => opt.MapFrom(src =>
                src.MainCategory != null
                ? new CategoryResponseDto
                {
                    CategoryId = src.MainCategory.CategoryId,
                    Name = src.MainCategory.Name,
                    ParentCategoryId = src.MainCategory.ParentCategoryId
                }
                : null))
            .ForMember(dest => dest.AdditionalCategories, opt => opt.MapFrom(src => src.AdditionalCategories.Select(c => new CategoryResponseDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                ParentCategoryId = c.ParentCategoryId
            }).ToList()))
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews.Select(r => new ReviewDto
            {
                Name = r.Name,
                Review = r.ReviewText,
                Rating = r.Rating,
                Date = r.Date.ToString("dd.MM.yyyy")
            }).ToList()))
            .ForMember(dest => dest.Characteristics, opt => opt.MapFrom(src => src.Characteristics.Select(c => new CharacteristicDto
            {
                Title = c.Title,
                Desc = c.Desc
            }).ToList()))
            .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.InStock));
    }
}
