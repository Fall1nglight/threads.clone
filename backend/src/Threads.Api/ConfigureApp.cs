using System.Reflection;
using Serilog;
using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api;

public static class ConfigureApp
{
    public static WebApplication Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseExceptionHandler();
        app.UseStatusCodePages();
        app.UseAuthentication();
        app.UseAuthorization();
        app.AddEndpoints();
        return app;
    }

    private static WebApplication AddEndpoints(this WebApplication app)
    {
        var assembly = typeof(Program).Assembly;

        var routers = assembly
            .GetTypes()
            .Where(x => x.IsClass && x.IsAssignableTo(typeof(IEndpointRouter)) && !x.IsAbstract);

        foreach (var router in routers)
        {
            var mapMethod = router.GetMethod(
                nameof(IEndpointRouter.MapRouter),
                BindingFlags.Public | BindingFlags.Static
            );

            if (mapMethod == null)
                continue;

            mapMethod.Invoke(null, [app]);
        }

        return app;
    }
}
