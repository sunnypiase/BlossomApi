using BlossomApi.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Filters;

namespace BlossomApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureGeneralServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Configure Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Add DbContext
            var connectionString = environment.IsDevelopment()
                ? "Host=localhost;Username=postgres;Password=root;Database=postgres;Port=5432;Pooling=true;"
                : ConnectionHelper.GetConnectionString(configuration);

            services.AddDbContext<BlossomContext>(opt => opt.UseNpgsql(connectionString));

            // Add Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Custom password validation rules
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<BlossomContext>()
            .AddDefaultTokenProviders();

            // Configure Cookie Settings
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Add Controllers
            services.AddControllers();

            // Add Memory Cache
            services.AddMemoryCache();

            // Configure CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins(
                            "http://localhost:3000",
                            "https://localhost:3000",
                            "http://fight-club-ivory.vercel.app",
                            "https://fight-club-ivory.vercel.app")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
        }
    }
}
