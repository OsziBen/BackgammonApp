using Application.GameSessions.Services.SessionCodeGenerator;
using Domain.GameLogic.Generators;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ExtensionMethods
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IMoveSequenceGenerator, MoveSequenceGenerator>();
            services.AddScoped<ISessionCodeGenerator, SessionCodeGenerator>();

            return services;
        }
    }
}
