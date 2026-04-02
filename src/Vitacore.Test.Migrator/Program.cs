using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vitacore.Test.Data.Postgres;

const int maxRetryCount = 10;
var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
    ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
    ?? "Production";

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = configuration["Application:DbConnectionString"]
    ?? throw new InvalidOperationException("Configuration value 'Application:DbConnectionString' is required.");

var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseNpgsql(connectionString)
    .UseSnakeCaseNamingConvention()
    .Options;
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});
var logger = loggerFactory.CreateLogger("Vitacore.Test.Migrator");

for (var attempt = 1; attempt <= maxRetryCount; attempt++)
{
    try
    {
        await using var dbContext = new AppDbContext(dbContextOptions);
        logger.LogInformation("Applying database migrations.");
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied successfully.");
        return;
    }
    catch (Exception ex) when (attempt < maxRetryCount)
    {
        logger.LogWarning(
            ex,
            "Database migration attempt {Attempt}/{MaxRetryCount} failed.",
            attempt,
            maxRetryCount);

        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}

await using (var dbContext = new AppDbContext(dbContextOptions))
{
    logger.LogInformation("Applying database migrations.");
    await dbContext.Database.MigrateAsync();
}
