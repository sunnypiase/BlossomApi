using AutoMapper;
using System.Reflection;

namespace BlossomApi.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}