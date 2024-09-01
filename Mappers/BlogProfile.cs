using AutoMapper;
using BlossomApi.Dtos.Blogs;
using BlossomApi.Models;

namespace BlossomApi.Mappers
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Blog, BlogResponseDto>();
            CreateMap<Blog, BlogWithProductsDto>();
            CreateMap<BlogCreateDto, Blog>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());
            CreateMap<BlogUpdateDto, Blog>()
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}
