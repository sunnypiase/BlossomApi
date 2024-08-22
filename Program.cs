using BlossomApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Configure Services
builder.Services.ConfigureGeneralServices(builder.Configuration, builder.Environment);
builder.Services.AddScopedServices();
builder.Services.AddAutoMapperProfiles();

var app = builder.Build();

// Configure Middleware
app.ConfigureMiddleware();

app.Run();
