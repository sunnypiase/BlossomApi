namespace BlossomApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            // Configure Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlossomApi v1"));

            // Development-Specific Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // General Middleware
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigins");
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None
            });

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Map Controllers
            app.MapControllers();

            // Environment-Specific Configuration
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "ENV")
            {
                app.Run("http://0.0.0.0:80");
            }
            else
            {
                Environment.SetEnvironmentVariable("ADMIN_SECRET", "admin_secret");
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Version 5.0");
                app.Run("http://0.0.0.0:8001");
            }
        }
    }
}
