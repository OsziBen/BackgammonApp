using Asp.Versioning;

namespace WebAPI.Extensions
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection ConfigureApiVersioning(
            this IServiceCollection services,
            IConfiguration config)
        {
            var versionString = config.GetValue<string>("ApiVersion") ??
                throw new InvalidOperationException("API version is not configured.");

            if (!Version.TryParse(versionString, out var version))
            {
                throw new FormatException("Invalid ApiVersioning format.");
            }

            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(version.Major, version.Minor);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            return services;
        }
    }
}
