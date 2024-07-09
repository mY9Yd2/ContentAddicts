using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using ContentAddicts.Api.Behaviors;
using ContentAddicts.Api.Contexts;
using ContentAddicts.Api.Policies;
using ContentAddicts.Api.Services;
using ContentAddicts.Api.Strategies;

using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Events;

var loggerConfiguration = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
        .WriteTo.Console();

// Serilog issue: https://github.com/serilog/serilog-aspnetcore/issues/289
Log.Logger = args.Contains("--environment=Development")
        ? loggerConfiguration.CreateLogger()
        : loggerConfiguration.CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .WriteTo.Console());
    builder.Services.AddControllers()
            .AddJsonOptions(cfg =>
                    {
                        cfg.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        cfg.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        cfg.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    });
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Content Addicts API",
                    Description = "Content Addicts is a place to discuss different types of content and its creators, mainly focusing on Youtube, X (formerly Twitter) and others.",
                    Contact = new OpenApiContact
                    {
                        Name = "GitHub Discussions",
                        Url = new Uri("//github.com/mY9Yd2/ContentAddicts/discussions")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("//github.com/mY9Yd2/ContentAddicts/blob/main/LICENSE.md")
                    }
                });

                options.CustomSchemaIds(SchemaIdStrategy.CleanStrategy);

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
    builder.Services.AddDbContext<AppDbContext>(options =>
            {
                string? connectionString = builder.Configuration["DefaultConnectionString"];

                if (connectionString is null) throw new ArgumentNullException(connectionString, "DefaultConnectionString cannot be null!");

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                options.EnableDetailedErrors();

                if (builder.Environment.IsDevelopment())
                {
                    options.LogTo(Console.WriteLine, LogLevel.Information);
                    options.EnableSensitiveDataLogging();
                }
            });

    ValidatorOptions.Global.DisplayNameResolver = ValidatorDisplayNamePolicy.CamelCase;

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }

    app.UseExceptionHandler();

    app.UseHttpsRedirection();

    app.MapControllers()
            .WithOpenApi();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program
{
    protected Program() { }
}
