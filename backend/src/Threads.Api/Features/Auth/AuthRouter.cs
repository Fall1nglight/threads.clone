using Threads.Api.Common;
using Threads.Api.Data.Shared.Interfaces;
using Threads.Api.Features.Auth.Endpoints;

namespace Threads.Api.Features.Auth;

public class AuthRouter : IEndpointRouter
{
    public static void MapRouter(IEndpointRouteBuilder builder)
    {
        var authRoute = builder.MapGroup("/auth").AllowAnonymous();

        authRoute.MapEndpoint<Login>();
        authRoute.MapEndpoint<Signup>();
    }
}
