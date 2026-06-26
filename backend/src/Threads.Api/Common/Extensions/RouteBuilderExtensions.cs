using Threads.Api.Common.Filters;
using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Common.Extensions;

public static class RouteBuilderExtensions
{
    /// <summary>
    /// Maps an endpoint to the specified <see cref="IEndpointRouteBuilder"/> using the endpoint's implementation
    /// of the <see cref="IEndpoint"/> interface.
    /// </summary>
    /// <typeparam name="TEndpoint">A type that implements the <see cref="IEndpoint"/> interface.</typeparam>
    /// <param name="builder">The <see cref="IEndpointRouteBuilder"/> to which the endpoint will be mapped.</param>
    /// <returns>The <see cref="IEndpointRouteBuilder"/> with the mapped endpoint.</returns>
    public static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder builder)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(builder);
        return builder;
    }

    public static RouteHandlerBuilder WithValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<RequestValidationFilter<TRequest>>().ProducesValidationProblem();
        return builder;
    }
}
