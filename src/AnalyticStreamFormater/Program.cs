using System.Text.Json.Serialization;
using AnalyticStreamFormater.Clients;
using AnalyticStreamFormater.Configuration;
using AnalyticStreamFormater.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Geosys.NotificationDataExporter.WebHook.Domain.JsonConverters;
using Geosys.NotificationDataExporter.WebHook.Domain.Models;
using Geosys.NotificationDataExporter.WebHook.Domain.Models.Validators;
using Geosys.NotificationDataExporter.WebHook.Domain.Services;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Mandatory for correct deserialization
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonObjectConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Dto Validation
builder.Services.AddTransient<IValidator<AnalyticsDto>, AnalyticsValidator>();
builder.Services.AddTransient<IValidator<List<AnalyticsDto>>, AnalyticsListValidator>();
builder.Services.AddFluentValidation();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// IdentityServer
builder.Services.Configure<IdentityServerConfiguration>(builder.Configuration.GetSection("IdentityServer"));
builder.Services.AddHttpClient<IIdentityServerClient, IdentityServerClient>();

// MapProduct
builder.Services.Configure<MapProductConfiguration>(builder.Configuration.GetSection("MapProduct"));
builder.Services.AddHttpClient<IMapProductClient, MapProductClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
