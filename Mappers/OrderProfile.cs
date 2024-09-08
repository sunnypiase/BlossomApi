using AutoMapper;
using BlossomApi.Dtos.Orders;
using BlossomApi.Models;

namespace BlossomApi.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mapping from Order to OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.DiscountFromPromocode + src.DiscountFromProductAction))
                .ForMember(dest => dest.TotalPriceWithDiscount, opt => opt.MapFrom(src => src.TotalPrice - (src.DiscountFromPromocode + src.DiscountFromProductAction)))
                .ForMember(dest => dest.DeliveryInfo, opt => opt.MapFrom(src => src.DeliveryInfo != null
                    ? new DeliveryInfoDto
                    {
                        City = src.DeliveryInfo.City,
                        DepartmentNumber = src.DeliveryInfo.DepartmentNumber
                    }
                    : null));

            // Mapping from Order to OrderDetailsDto
            CreateMap<Order, OrderDetailsDto>()
                .ForMember(dest => dest.Promocode, opt => opt.MapFrom(src => src.Promocode != null ? src.Promocode.Code : string.Empty))
                .ForMember(dest => dest.PromocodeId, opt => opt.MapFrom(src => src.PromocodeId))
                .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.DiscountFromPromocode + src.DiscountFromProductAction))
                .ForMember(dest => dest.ProductsDiscount, opt => opt.MapFrom(src => src.DiscountFromProductAction))
                .ForMember(dest => dest.TotalPriceWithDiscount, opt => opt.MapFrom(src => src.TotalPrice - (src.DiscountFromPromocode + src.DiscountFromProductAction)))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ShoppingCart.ShoppingCartProducts.Select(scp => new ProductInOrderDto
                {
                    ProductName = scp.Product.Name,
                    ProductId = scp.Product.ProductId,
                    Article = scp.Product.Article,
                    Quantity = scp.Quantity,
                    UnitPrice = scp.Product.Price,
                    Discount = scp.Product.Discount,
                    Total = scp.Quantity * (scp.Product.Price - (scp.Product.Price * scp.Product.Discount / 100))
                }).ToList()))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.DeliveryInfo.City))
                .ForMember(dest => dest.DepartmentNumber, opt => opt.MapFrom(src => src.DeliveryInfo.DepartmentNumber));

            // Mapping from DeliveryInfo to DeliveryInfoDto
            CreateMap<DeliveryInfo, DeliveryInfoDto>();

            // Mapping for OrderStatusOptionDto
            CreateMap<OrderStatus, OrderStatusOptionDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.ToUkrainianName()));
        }
    }
}
