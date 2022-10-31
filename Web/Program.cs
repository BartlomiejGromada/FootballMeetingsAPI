using Contracts.MappingProfiles;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Persistence;
using Persistence.Repositories;
using Services;
using Services.Abstractions;
using Services.Validators;
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
builder.Services.AddAutoMapper();

builder.Services.ConfigureJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFootballPitchesService, FootballPitchesService>();
builder.Services.AddScoped<IFootballMatchesService, FootballMatchesService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureCors(builder.Configuration);

builder.Services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();
builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

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
