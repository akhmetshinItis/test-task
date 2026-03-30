using Vitacore.Test.Web;
using Vitacore.Test.Web.Configuration;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
const string corsPolicyName = "AllowedOrigins";

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins(
                "https://dev.api.d4rq.ru", //TODO вынести в конфиги
                "https://api.d4rq.ru")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            var prefix = httpReq.Headers["X-Forwarded-Prefix"].FirstOrDefault() ?? "";

            swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer
                {
                    Url = $"{httpReq.Scheme}://{httpReq.Host}{prefix}"
                }
            };
        });
    });
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyName);

app.MapControllers();

app.Run();