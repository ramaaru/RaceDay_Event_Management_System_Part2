using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure listening URLs. If ASPNETCORE_URLS is set use that, otherwise fall back to ephemeral loopback ports
// This avoids "address already in use" failures when running multiple instances locally.
var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://127.0.0.1:0;https://127.0.0.1:0";
builder.WebHost.UseUrls(urls);

builder.Services.AddDbContext<RaceDayDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RaceDayConnection")));
builder.Services.AddScoped<PasswordService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30); options.Cookie.HttpOnly = true; options.Cookie.IsEssential = true; options.Cookie.SameSite = SameSiteMode.Lax; });
// Ensure tests using TestServer (which bind to port 0) don't cause Https redirection to throw.
builder.Services.AddHttpsRedirection(options => { options.HttpsPort = 443; });
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
// Log incoming requests to help diagnose test failures
app.Use(async (context, next) =>
{
    app.Logger.LogInformation("Incoming request {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
});
app.UseSession();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
