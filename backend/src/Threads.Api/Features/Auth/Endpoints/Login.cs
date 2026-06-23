using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Threads.Api.Common;
using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Features.Auth.Endpoints;

public class Login : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/login", Handle).WithValidation<Request>();
    }

    public record Request(string Email, string Password);

    public record Response(string AccessToken);

    public class LoginValidator : AbstractValidator<Request>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).EmailAddress().MaximumLength(100);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(40);
        }
    }

    private static async Task<Ok<Response>> Handle(Request request)
    {
        // do some work
        return TypedResults.Ok(new Response("test"));
    }
}
