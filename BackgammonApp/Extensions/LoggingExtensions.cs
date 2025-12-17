using Serilog;

namespace WebAPI.Extensions
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder ConfigureSerilog(
            this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, loggerConfig) =>
            {
                loggerConfig
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext();
            });

            return builder;
        }
    }
}
