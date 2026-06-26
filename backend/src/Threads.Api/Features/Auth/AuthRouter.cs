using Threads.Api.Common.Extensions;
using Threads.Api.Data.Shared.Interfaces;
using Threads.Api.Features.Auth.Endpoints;

namespace Threads.Api.Features.Auth;

public class AuthRouter : IEndpointRouter
{
    public static void MapRouter(IEndpointRouteBuilder builder)
    {
        var authRoute = builder.MapGroup("/auth");

        authRoute
            .AllowAnonymous()
            .MapEndpoint<Login>()
            .MapEndpoint<Signup>()
            .MapEndpoint<RenewToken>();

        authRoute.RequireAuthorization().MapEndpoint<Logout>().MapEndpoint<LogoutAll>();
    }
}
