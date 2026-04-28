using Application.ExtensionMethods;
using Infrastructure.Data;
using Infrastructure.ExtensionMethods;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebAPI.Extensions;
using WebAPI.Hubs;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddAppConfig(builder.Configuration);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.ConfigureApiVersioning(builder.Configuration);
builder.Services.ConfigureMediatRServices();
builder.Services.AddAuthorizationServices();
builder.Services.AddSwaggerExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddRealtimeServices();

builder.Services
    .AddApplication()
    .AddPersistence()
    .AddSystemServices();

var app = builder.Build();

app.ConfigureSwaggerExplorer();

app.UseHttpsRedirection();

app.UseRouting();

app.ConfigureCORS();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseConfiguredSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameSessionHub>("/hubs/game-session").RequireAuthorization();

app.Run();
