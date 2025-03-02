using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttPublisher.Entities;
using MqttPublisher.Repositories;
using MqttPublisher.Services;
using MqttPublisher.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Management.EventGrid.Models;
using MongoDB.Driver;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add cors service
builder.Services.AddCors();

// Add Configuration file
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<AzureCredentials>(builder.Configuration.GetSection("AzureCredentials"));
builder.Services.Configure<AllowedCors>(builder.Configuration.GetSection("AllowedCors"));

// Adding Controllers
builder.Services.AddControllers();

//Adding services
builder.Services.AddSingleton<IMongoDBService, MongoDBService>();
builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
builder.Services.AddSingleton<IDomainRepository, DomainRepository>();
builder.Services.AddSingleton<IBrokerService, BrokerService>();
builder.Services.AddSingleton<IAppLogger, AppLogger>();

var app = builder.Build();

// start the service
StartService();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors(options =>
    options.WithOrigins(builder.Configuration.GetSection("AllowedCors:Origins").Get<string[]>())
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials());
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();
app.Run();


void StartService()
{
    app.Services.GetRequiredService<IBrokerService>();
}
