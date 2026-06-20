using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Features.Auth.Endpoints;

public class Signup : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/signup", () => "signup page");
    }
}
