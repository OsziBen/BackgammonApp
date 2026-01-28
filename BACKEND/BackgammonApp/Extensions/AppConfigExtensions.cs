namespace WebAPI.Extensions
{
    public static class AppConfigExtensions
    {
        private const string DevCorsPolicy = "DevCors";

        public static IServiceCollection AddAppConfig(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DevCorsPolicy, policy =>
                {
                    policy
                    .WithOrigins(
                        "http://localhost:4200",
                        "https://localhost:4200"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            return services;
        }

        public static WebApplication ConfigureCORS(this WebApplication app)
        {
            app.UseCors(DevCorsPolicy);

            return app;
        }
    }
}
