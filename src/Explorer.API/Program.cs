using Explorer.API.Controllers.Proto;
using Explorer.API.Middleware;
using Explorer.API.Startup;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);
const string corsPolicy = "_corsPolicy";
builder.Services.ConfigureCors(corsPolicy);
builder.Services.ConfigureAuth();

builder.Services.RegisterModules();
builder.Services.AddGrpc();

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