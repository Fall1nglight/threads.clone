using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Threads.Api.Common;
using Threads.Api.Data.Shared;
using Threads.Api.Data.Users;

namespace Threads.Api;

public static class ConfigureServices
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
        AddIdentity(builder);
        AddProblemDetails(builder);
        AddGlobalErrorHandler(builder);
        AddLogger(builder);
        AddValidators(builder);
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

    private static void AddIdentity(WebApplicationBuilder builder)
    {
        builder
            .Services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();
    }

    /// <summary>
    /// Configures and adds Problem Details services to the application builder,
    /// enabling standardized error responses according to RFC 7807.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    private static void AddProblemDetails(WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();
    }

    /// <summary>
    /// Configures and adds a global error handler to the application builder.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    private static void AddGlobalErrorHandler(WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    /// <summary>
    /// Configures and adds the logging services to the application builder.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    private static void AddLogger(WebApplicationBuilder builder)
    {
        // todo: add Azure application insights
        builder.Services.AddSerilog(
            (services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(builder.Configuration)
                    .ReadFrom.Services(services);
            }
        );
    }

    /// <summary>
    /// Configures and adds FluentValidation validators to the application builder
    /// by scanning the specified assembly for validator implementations.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> instance to configure.</param>
    private static void AddValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    }
}
