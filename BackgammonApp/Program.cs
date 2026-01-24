using Application.ExtensionMethods;
using Infrastructure.Data;
using Infrastructure.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Hubs;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddControllers();

builder.Services.ConfigureApiVersioning(builder.Configuration);
builder.Services.ConfigureMediatRServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddRealtimeServices();

builder.Services
    .AddApplication()
    .AddPersistence()
    .AddSystemServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseConfiguredSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<GameSessionHub>("/hubs/game-session");

app.Run();
