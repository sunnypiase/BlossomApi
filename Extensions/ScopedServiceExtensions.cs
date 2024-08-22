using BlossomApi.Repositories;
using BlossomApi.Services;

namespace BlossomApi.Extensions
{
    public static class ScopedServiceExtensions
    {
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<CategoryService>();
            services.AddScoped<IShownProductRepository, ShownProductRepository>();
            services.AddScoped<ImageService>();
            services.AddScoped<ProductQueryService>();
            services.AddScoped<ProductImageService>();
            services.AddScoped<ProductImportService>();
            services.AddScoped<ProductRecommendationService>();
        }
    }
}