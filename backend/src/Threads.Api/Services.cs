using Microsoft.EntityFrameworkCore;
using Threads.Api.Data.Shared;

namespace Threads.Api;

public static class Services
{
    /// <summary>
    /// Configures and adds required services to the application builder.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    /// <returns>The updated <see cref="WebApplicationBuilder"/> instance.</returns>
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        // note: return WebApplicationBuilder instead of void to enable method chaining
        AddDatabase(builder);
        return builder;
    }

    /// <summary>
    /// Configures and adds the database context to the application builder.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    private static void AddDatabase(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(option =>
        {
            option.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
        });
    }
}
