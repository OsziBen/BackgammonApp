using Serilog;

namespace WebAPI.Extensions
{
    public static class SerilogRequestLoggingExtensions
    {
        public static IApplicationBuilder UseConfiguredSerilogRequestLogging(
            this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate =
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set(
                        "ClientIP",
                        httpContext.Connection.RemoteIpAddress?.ToString());

                    diagnosticContext.Set(
                        "User",
                        httpContext.User?.Identity?.IsAuthenticated == true
                            ? httpContext.User.Identity.Name
                            : "Anonymous");
                };
            });

            return app;
        }
    }
}
