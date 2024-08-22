using AutoMapper;
using BlossomApi.Dtos;
using BlossomApi.Models;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => new CategoryResponseDto
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
            }).ToList()));
    }
}
