using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Common;

public static class RouteBuilderExtension
{
    public static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder builder)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(builder);
        return builder;
    }
}
