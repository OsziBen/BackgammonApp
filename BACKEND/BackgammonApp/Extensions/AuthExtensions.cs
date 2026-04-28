using Domain.GroupRole;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Authorization;
using WebAPI.Constants;

namespace WebAPI.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<JwtOptions>(
                config.GetSection(nameof(JwtOptions)));

            var jwtOptions = config
                .GetSection(nameof(JwtOptions))
                .Get<JwtOptions>()
                ?? throw new InvalidOperationException("JwtOptions not configured properly.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        //ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions!.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken)
                                && path.StartsWithSegments(HubRoutes.GameSession))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                }
            );

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.GroupOwner,
                    policy => policy.Requirements.Add(
                        new GroupRoleRequirement(GroupRoleConstants.Owner)));

                options.AddPolicy(Policies.GroupModerator,
                    policy => policy.Requirements.Add(
                        new GroupRoleRequirement(
                            GroupRoleConstants.Owner,
                            GroupRoleConstants.Moderator)));

                options.AddPolicy(Policies.GroupMember,
                    policy => policy.Requirements.Add(
                        new GroupRoleRequirement(
                            GroupRoleConstants.Owner,
                            GroupRoleConstants.Moderator,
                            GroupRoleConstants.Member)));

                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }

        public static IServiceCollection AddAuthorizationServices(
            this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, GroupRoleHandler>();

            return services;
        }
    }
}
