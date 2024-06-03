using Explorer.API.Controllers.Proto;
using Explorer.API.Middleware;
using Explorer.API.Startup;
using Microsoft.Extensions.FileProviders;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using OpenTelemetry.Instrumentation.Process;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();
builder.Services.RegisterModules();
builder.Services.AddGrpc();

const string serviceName = "api";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation(options =>
        {
            options.SetDbStatementForText = true;
        })
        .AddConsoleExporter()
        .AddZipkinExporter(zipkinOptions =>
        {
            zipkinOptions.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
        })
        .AddJaegerExporter(jaegerOptions =>
        {
            jaegerOptions.AgentHost = "localhost";
            jaegerOptions.AgentPort = 6831;
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddConsoleExporter());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseMiddleware<GrpcLoggingMiddleware>();

app.UseRouting();
app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<AuthenticationProtoController>();
app.MapGrpcService<UserProtoController>();
app.MapGrpcService<PersonProtoController>();

app.MapGet("/", () => "Worker Process Name : " + System.Diagnostics.Process.GetCurrentProcess().ProcessName);

app.Run();

// Required for automated tests
namespace Explorer.API
{
    public partial class Program { }
}
