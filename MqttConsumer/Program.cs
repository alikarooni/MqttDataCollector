using MqttConsumer.Configuration;
using MqttConsumer.Services;
using MqttConsumer.Repositories;
using MqttConsumer.SignalRHub;
using MqttPublisher.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();

// Add Configuration file
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<AllowedCors>(builder.Configuration.GetSection("AllowedCors"));


// Adding Controllers
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSignalR();

//Adding services
builder.Services.AddSingleton<IMongoDBService, MongoDBService>();
builder.Services.AddSingleton<IMqttEventRepository, MqttEventRepository>();
builder.Services.AddSingleton<IDomainRepository, DomainRepository>();
builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
builder.Services.AddSingleton<IMqttHubService, MqttHubService>();
builder.Services.AddSingleton<IMqttEventProcessorService, MqttEventProcessorService>();

var app = builder.Build();

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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MqttHub>("/signalrservice");
});

app.Run();