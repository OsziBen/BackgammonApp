using Application.Shared;
using FluentValidation;

namespace WebAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection ConfigureMediatRServices(
            this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(
                    typeof(ValidationBehavior<,>).Assembly);

                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(
                typeof(ValidationBehavior<,>).Assembly);

            return services;
        }
    }
}
