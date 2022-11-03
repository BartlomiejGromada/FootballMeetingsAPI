using Contracts;
using Contracts.MappingProfiles;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Persistence;
using Persistence.Repositories;
using Services;
using Sieve.Models;
using Sieve.Services;
using Web.Extensions;
using Web.Middlewares;
using Web.Sieve;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureApiVersion();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("FootballMeetingsConnectionString"));
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddAutoMapper();

builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.AddApplications();
builder.Services.AddValidators();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddSieve(builder.Configuration.GetSection("Sieve"));

builder.Services.AddScoped<ErrorHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("FrontendClient");

app.SeedData().Run();
