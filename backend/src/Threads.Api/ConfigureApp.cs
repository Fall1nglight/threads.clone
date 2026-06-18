namespace Threads.Api;

public static class ConfigureApp
{
    public static WebApplication Configure(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        return app;
    }
}
