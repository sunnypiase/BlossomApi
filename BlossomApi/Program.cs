using BlossomApi.DB;
using BlossomApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<CategoryService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
// Determine the environment and set the connection string accordingly
string connectionString;

if (builder.Environment.IsDevelopment())
{
    // Local development connection string
    connectionString = "Server=localhost;Database=BlossomDb;Trusted_Connection=True;TrustServerCertificate=True;";
}
else
{
    // Load environment-specific appsettings
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

    // Log the connection string for debugging
    logger.LogInformation($"DB_HOST: {dbHost}");
    logger.LogInformation($"DB_NAME: {dbName}");
    logger.LogInformation($"DB_SA_PASSWORD: {dbPassword}");

    connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True";
    logger.LogInformation($"Connection String: {connectionString}");
}

// Add services to the container.
builder.Services.AddDbContext<BlossomContext>(opt => opt.UseSqlServer(connectionString));
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
logger.LogInformation("Version 1.8");

app.Run("http://0.0.0.0:80");