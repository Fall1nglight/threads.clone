using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Features.Auth.Endpoints;

public class Login : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/login", () => "login page");
    }
}
