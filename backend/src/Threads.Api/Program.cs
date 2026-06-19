using Serilog;
using Threads.Api;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

try
{
    Log.Information("Starting Threads.Api");

    var builder = WebApplication.CreateBuilder(args);
    builder.AddServices();

    var app = builder.Build();
    app.MapGet("/", () => "Homepage");
    app.Configure();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
