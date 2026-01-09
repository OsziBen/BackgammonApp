using Application.GameSessions.Services.SessionCodeGenerator;
using Application.Interfaces;
using Domain.GameLogic.Generators;
using Infrastructure.Data;
using Infrastructure.Realtime.Factories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WebAPI.Extensions;
using WebAPI.Hubs;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.ConfigureSerilog();

builder.Services.AddControllers();

builder.Services.ConfigureApiVersioning(builder.Configuration);
builder.Services.ConfigureMediatRServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDbContext")));

builder.Services.AddRealtimeServices();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupMembershipRoleRepository, GroupMembershipRoleRepository>();
builder.Services.AddScoped<IDiceService, DiceService>();
builder.Services.AddScoped<IBoardStateFactory, BoardStateFactory>();
builder.Services.AddScoped<IMoveSequenceGenerator, MoveSequenceGenerator>();
builder.Services.AddScoped<ISessionCodeGenerator, SessionCodeGenerator>();

// TODO: extension method(s)

var app = builder.Build();

// Configure the HTTP request pipeline.
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
