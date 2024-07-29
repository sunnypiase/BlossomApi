using BlossomApi.DB;
using BlossomApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
{
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
    services.AddScoped<CategoryService>();
    services.AddSingleton<ILoggerFactory, LoggerFactory>();
    services.AddLogging(config =>
    {
        config.ClearProviders();
        config.AddConsole();
    });

    var connectionString = GetConnectionString(configuration, environment);
    services.AddDbContext<BlossomContext>(opt => opt.UseNpgsql(connectionString));
    services.AddAuthorization();
    services.AddIdentityApiEndpoints<IdentityUser>()
        .AddEntityFrameworkStores<BlossomContext>();
    services.AddControllers();

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

string GetConnectionString(IConfiguration configuration, IWebHostEnvironment environment)
    => environment.IsDevelopment()
        ? ConnectionHelper.GetConnectionString(configuration)
        : "Host=localhost;Username=postgres;Password=root;Database=postgres;Port=5432;Pooling=true;";

void ConfigureMiddleware(WebApplication webApp)
{
    var logger = webApp.Services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

    webApp.UseSwagger();
    webApp.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlossomApi v1"));

    if (webApp.Environment.IsDevelopment())
    {
        webApp.UseDeveloperExceptionPage();
    }

    webApp.UseHttpsRedirection();
    webApp.UseRouting();

    webApp.UseCors("AllowAll");

    webApp.UseAuthorization();
    webApp.MapControllers();

    webApp.Run(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "ENV" ? "http://0.0.0.0:80" : "http://0.0.0.0:8001");

    logger.LogInformation("Version 4.0");
}
