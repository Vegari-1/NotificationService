using NotificationService.Service.Interface;
using NotificationService.Service;
using OpenTracing;
using Jaeger.Reporters;
using Jaeger;
using Jaeger.Senders.Thrift;
using Jaeger.Samplers;
using OpenTracing.Contrib.NetCore.Configuration;
using OpenTracing.Util;
using Prometheus;
using PostService.Repository;
using Microsoft.EntityFrameworkCore;
using NotificationService.Repository.Interface;
using NotificationService.Repository;
using NotificationService.Middlewares.Exception;
using BusService;
using Microsoft.Extensions.Options;
using NotificationService.Messaging;
using NotificationService.Service.Interface.Sync;
using NotificationService.Service.Sync;
using NotificationService.Repository.Interface.Sync;
using NotificationService.Repository.Sync;
using NotificationService.Middlewares.Events;

var builder = WebApplication.CreateBuilder(args);

// Environment variables
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<AppConfig>(
    builder.Configuration.GetSection("AppConfig"));

// Nats
builder.Services.Configure<MessageBusSettings>(builder.Configuration.GetSection("Nats"));
builder.Services.AddSingleton<IMessageBusSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MessageBusSettings>>().Value);
builder.Services.AddSingleton<IMessageBusService, MessageBusService>();
builder.Services.AddHostedService<NotificationMessageBusService>();

// DB_HOST from Docker-Compose or Local if null
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
// Postgres
if (dbHost == null)
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DislinktDbConnection"),
            x => x.MigrationsHistoryTable("__MigrationsHistory", "notification")));
else
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(dbHost, x => x.MigrationsHistoryTable("__MigrationsHistory", "notification")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Repositories
builder.Services.AddScoped<INotificationConfigRepository, NotificationConfigRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IConnectionRepository, ConnectionRepository>();

// Services
builder.Services.AddScoped<INotificationService, NotificationService.Service.NotificationService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Sync Services
builder.Services.AddScoped<IProfileSyncService, ProfileSyncService>();
builder.Services.AddScoped<IConnectionSyncService, ConnectionSyncService>();
builder.Services.AddScoped<IMessageSyncService, MessageSyncService>();
builder.Services.AddScoped<IPostSyncService, PostSyncService>();
builder.Services.AddScoped<IEventSyncService, EventSyncService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddOpenTracing();

builder.Services.AddSingleton<ITracer>(sp =>
{
    var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    .WithSender(new UdpSender("host.docker.internal", 6831, 0))
                    .Build();
    var tracer = new Tracer.Builder(serviceName)
        // The constant sampler reports every span.
        .WithSampler(new ConstSampler(true))
        // LoggingReporter prints every reported span to the logging framework.
        .WithLoggerFactory(loggerFactory)
        .WithReporter(reporter)
        .Build();

    GlobalTracer.Register(tracer);

    return tracer;
});

builder.Services.Configure<HttpHandlerDiagnosticOptions>(options =>
        options.OperationNameResolver =
            request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");

var app = builder.Build();

// Run all migrations only on Docker container
if (dbHost != null)
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseEventSenderMiddleware();

// Prometheus metrics
app.UseMetricServer();

app.Run();

namespace NotificationService
{
    public partial class Program { }
}
