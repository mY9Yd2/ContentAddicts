using System.Reflection;

using ContentAddicts.Api.Contexts;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
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

var app = builder.Build();

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

app.UseHttpsRedirection();

app.MapControllers()
        .WithOpenApi();

await app.RunAsync();
