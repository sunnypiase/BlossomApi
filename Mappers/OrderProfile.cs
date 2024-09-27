using AutoMapper;
using BlossomApi.Dtos.Orders;
using BlossomApi.Models;
using BlossomApi.Extensions;
using System.Linq;
using BlossomApi.Dtos;

namespace BlossomApi.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Маппінг з Order до OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.TotalDiscount))
                .ForMember(dest => dest.TotalPriceWithDiscount, opt => opt.MapFrom(src => src.TotalPriceWithDiscount))
                .ForMember(dest => dest.DeliveryInfo, opt => opt.MapFrom(src => src.DeliveryInfo))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname));

            // Маппінг з DeliveryInfo до DeliveryInfoDto
            CreateMap<DeliveryInfo, DeliveryInfoDto>();

            // Маппінг з Order до OrderDetailsDto
            CreateMap<Order, OrderDetailsDto>()
                .ForMember(dest => dest.Promocode, opt => opt.MapFrom(src => src.Promocode != null ? src.Promocode.Code : string.Empty))
                .ForMember(dest => dest.PromocodeId, opt => opt.MapFrom(src => src.PromocodeId))
                .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.TotalDiscount))
                .ForMember(dest => dest.ProductsDiscount, opt => opt.MapFrom(src => src.DiscountFromProductAction))
                .ForMember(dest => dest.TotalPriceWithDiscount, opt => opt.MapFrom(src => src.TotalPriceWithDiscount))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ShoppingCart.ShoppingCartProducts))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.DeliveryInfo.City))
                .ForMember(dest => dest.DepartmentNumber, opt => opt.MapFrom(src => src.DeliveryInfo.DepartmentNumber))
                .ForMember(dest => dest.DiscountFromCashback, opt => opt.MapFrom(src => src.DiscountFromCashback))
                .ForMember(dest => dest.CashbackEarned, opt => opt.MapFrom(src => src.CashbackEarned));

            // Маппінг з ShoppingCartProduct до ProductInOrderDto
            CreateMap<ShoppingCartProduct, ProductInOrderDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.Article, opt => opt.MapFrom(src => src.Product.Article))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Product.Discount))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Quantity * (src.Product.Price - (src.Product.Price * src.Product.Discount / 100))));

            // Маппінг для OrderStatusOptionDto
            CreateMap<OrderStatus, OrderStatusOptionDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.ToUkrainianName()));

            CreateMap<Order, UserOrderSummaryDto>();

            // Mapping from ShoppingCartProduct to ProductInOrderDetailDto, including MainCategory
            CreateMap<ShoppingCartProduct, ProductInOrderDetailDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductByAmountPrice, opt => opt.MapFrom(src => src.Quantity * src.Product.Price))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.Images.FirstOrDefault())) // Adjust this field to match actual image logic
                .ForMember(dest => dest.MainCategory, opt => opt.MapFrom(src => src.Product.MainCategory));

        }
    }
}
