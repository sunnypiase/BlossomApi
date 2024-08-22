using AutoMapper;

namespace BlossomApi.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductProfile));
        }
    }
}