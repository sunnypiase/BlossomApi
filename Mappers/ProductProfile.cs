using AutoMapper;
using BlossomApi.Dtos;
using BlossomApi.Dtos.Characteristic;
using BlossomApi.Models;
using System.Reflection.PortableExecutable;

namespace BlossomApi.Mappers
{
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
                    Desc = c.Desc,
                    CharacteristicId = c.CharacteristicId
                }).ToList()));

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.MainCategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.NameEng, opt => opt.Condition(src => src.NameEng != null))
                .ForMember(dest => dest.Images, opt => opt.Condition(src => src.Images != null))
                .ForMember(dest => dest.Brand, opt => opt.Condition(src => src.Brand != null))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price.HasValue))
                .ForMember(dest => dest.Discount, opt => opt.Condition(src => src.Discount.HasValue))
                .ForMember(dest => dest.AvailableAmount, opt => opt.Condition(src => src.AvailableAmount.HasValue))
                .ForMember(dest => dest.Article, opt => opt.Condition(src => src.Article != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.Ingridients, opt => opt.Condition(src => src.Ingridients != null))
                .ForMember(dest => dest.IsNew, opt => opt.Condition(src => src.IsNew.HasValue))
                .ForMember(dest => dest.IsHit, opt => opt.Condition(src => src.IsHit.HasValue))
                .ForMember(dest => dest.IsShown, opt => opt.Condition(src => src.IsShown.HasValue))
                .ForMember(dest => dest.UnitOfMeasurement, opt => opt.Condition(src => src.UnitOfMeasurement != null))
                .ForMember(dest => dest.Group, opt => opt.Condition(src => src.Group != null))
                .ForMember(dest => dest.Type, opt => opt.Condition(src => src.Type != null))
                .ForMember(dest => dest.ManufacturerBarcode, opt => opt.Condition(src => src.ManufacturerBarcode != null))
                .ForMember(dest => dest.UKTZED, opt => opt.Condition(src => src.UKTZED != null))
                .ForMember(dest => dest.Markup, opt => opt.Condition(src => src.Markup.HasValue))
                .ForMember(dest => dest.PurchasePrice, opt => opt.Condition(src => src.PurchasePrice.HasValue))
                .ForMember(dest => dest.VATRate, opt => opt.Condition(src => src.VATRate.HasValue))
                .ForMember(dest => dest.ExciseTaxRate, opt => opt.Condition(src => src.ExciseTaxRate.HasValue))
                .ForMember(dest => dest.PensionFundRate, opt => opt.Condition(src => src.PensionFundRate.HasValue))
                .ForMember(dest => dest.VATLetter, opt => opt.Condition(src => src.VATLetter != null))
                .ForMember(dest => dest.ExciseTaxLetter, opt => opt.Condition(src => src.ExciseTaxLetter != null))
                .ForMember(dest => dest.MetaKeys, opt => opt.Condition(src => src.MetaKeys != null))
                .ForMember(dest => dest.MetaDescription, opt => opt.Condition(src => src.MetaDescription != null))
                .ForMember(dest => dest.PensionFundLetter, opt => opt.Condition(src => src.PensionFundLetter != null))
                .ForMember(dest => dest.DocumentQuantity, opt => opt.Condition(src => src.DocumentQuantity.HasValue))
                .ForMember(dest => dest.ActualQuantity, opt => opt.Condition(src => src.ActualQuantity.HasValue));

            // Map from ProductCreateDto to Product
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore()) // Handled separately
                .ForMember(dest => dest.Characteristics, opt => opt.Ignore()); // Handled separately


            CreateMap<Category, CategoryResponseDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentCategoryId));
        }
    }
}