using BlossomApi.DB;
using BlossomApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ImageService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

// Determine the environment and set the connection string accordingly
var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "ENV"
    ? ConnectionHelper.GetConnectionString(builder.Configuration)
    : "Host=localhost;Username=postgres;Password=root;Database=postgres;Port=5432;Pooling=true;";

// Log the connection string for debugging
logger.LogInformation($"Connection String: {connectionString}");

// Add services to the container.
builder.Services.AddDbContext<BlossomContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddAuthorization();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BlossomContext>()
    .AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddControllers();

// Add CORS services
builder.Services.AddCors(options =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlossomApi v1"));

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

// Enable CORS
app.UseCors("AllowSpecificOrigins");

// Apply the cookie policy
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.None
});

app.UseAuthentication();  // Add this line to enable authentication
app.UseAuthorization();
app.MapControllers();

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "ENV")
{
    app.Run("http://0.0.0.0:80");
}
else
{
    Environment.SetEnvironmentVariable("ADMIN_SECRET", "admin_secret");
    logger.LogInformation("Version 5.0");
    app.Run("http://0.0.0.0:8001");
}
