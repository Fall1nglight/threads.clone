using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Threads.Api.Common;
using Threads.Api.Data.Shared;
using Threads.Api.Data.Shared.Interfaces;
using Threads.Api.Data.Users;
using Threads.Api.Features.Auth.Services.JwtProvider;

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
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]")
                .Matches("[^a-zA-Z0-9]");
        }
    }

    private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handle(
        Request request,
        AppDbContext db,
        UserManager<User> userManager,
        IJwtProvider jwtProvider,
        CancellationToken cancellationToken
    )
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return TypedResults.Unauthorized();

        var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
            return TypedResults.Unauthorized();

        var accessToken = await jwtProvider.GenerateJwt(user);
        var response = new Response(accessToken);

        await transaction.CommitAsync(cancellationToken);
        return TypedResults.Ok(response);
    }
}
