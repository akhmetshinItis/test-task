using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using Vitacore.Test.Infrastructure;
using Vitacore.Test.Infrastructure.Background.Jobs;
using Vitacore.Test.Web;
using Vitacore.Test.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);
const string corsPolicyName = "AllowedOrigins";
var hangfireOptions = builder.Configuration
    .GetSection(HangfireOptions.SectionName)
    .Get<HangfireOptions>()
    ?? throw new InvalidOperationException($"Configuration section '{HangfireOptions.SectionName}' is required.");

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    const string schemeId = "bearer";

    options.SwaggerDoc("v1", new()
    {
        Title = "Vitacore.Test API",
        Version = "v1"
    });

    options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "JWT Authorization header using the Bearer scheme.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(document => new()
    {
        [new OpenApiSecuritySchemeReference(schemeId, document)] = []
    });
});

var startup = new Startup(builder.Configuration, hangfireOptions);
startup.ConfigureServices(builder.Services);
startup.ConfigureHangfire(builder.Services);

var app = builder.Build();

await app.Services.SeedIdentityAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (hangfireOptions.EnableDashboard)
{
    app.UseHangfireDashboard(hangfireOptions.DashboardPath);
}

RecurringJob.AddOrUpdate<CompleteEndedAuctionsJob>(
    "auctions:complete-ended",
    job => job.ExecuteAsync(),
    Cron.Minutely());

RecurringJob.AddOrUpdate<CleanupExpiredLotsJob>(
    "lots:cleanup-expired",
    job => job.ExecuteAsync(),
    Cron.Daily());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}
app.UseCors(corsPolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
