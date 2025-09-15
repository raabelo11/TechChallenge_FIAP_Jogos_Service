using AutoMapper;
using Jogos.ApiService.Middleware;
using Jogos.Service.Application.Configurations;
using Jogos.Service.Application.Logging;
using Jogos.Service.Application.Mappings;
using Jogos.Service.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddProvider(new CustomerLoggerProvider(new CustomLoggerProviderConfiguration { LogLevel = LogLevel.Information }));
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseLogging();

app.MapControllers();

app.Run();
