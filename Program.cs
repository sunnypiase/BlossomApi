using BlossomApi.DB;
using BlossomApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
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
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

// Determine the environment and set the connection string accordingly
string connectionString;

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "ENV")
{
    // Local development connection string
    connectionString = ConnectionHelper.GetConnectionString(builder.Configuration);
}
else
{
    connectionString = BuildConnectionString("postgresql://postgres:ZIZkykDYNDRGCaEdThFrNRaTPpzqLSPp@monorail.proxy.rlwy.net:39147/railway");
    // Use the connection helper to build the connection string from environment variables
}

// Log the connection string for debugging
logger.LogInformation($"Connection String: {connectionString}");

// Add services to the container.
builder.Services.AddDbContext<BlossomContext>(opt => opt.UseNpgsql(connectionString));
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<BlossomContext>();
builder.Services.AddControllers();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
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
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.Run("http://0.0.0.0:8001");
}
logger.LogInformation("Version 3.1");

app.Run("http://0.0.0.0:80");
static string BuildConnectionString(string databaseUrl)
{
    var databaseUri = new Uri(databaseUrl);
    var userInfo = databaseUri.UserInfo.Split(':');
        
    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = databaseUri.LocalPath.TrimStart('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    return builder.ToString();
}