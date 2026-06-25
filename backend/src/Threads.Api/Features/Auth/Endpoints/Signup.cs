using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Threads.Api.Common;
using Threads.Api.Data.Shared;
using Threads.Api.Data.Shared.Interfaces;
using Threads.Api.Data.Users;
using Threads.Api.Features.Auth.Services.JwtProvider;

namespace Threads.Api.Features.Auth.Endpoints;

public class Signup : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/signup", Handle).WithSummary("Registers user").WithValidation<Request>();
    }

    public record Request(
        string Email,
        string Username,
        string Password,
        bool IsPrivate,
        string? Bio
    );

    public record Response(string AccessToken);

    public class SignupValidator : AbstractValidator<Request>
    {
        public SignupValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);

            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30)
                .Matches("^[a-zA-Z0-9_.]+$");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]")
                .Matches("[^a-zA-Z0-9]");

            RuleFor(x => x.Bio).MaximumLength(300);
        }
    }

    private static async Task<Results<Ok<Response>, BadRequest<IEnumerable<IdentityError>>>> Handle(
        Request request,
        AppDbContext db,
        UserManager<User> userManager,
        IJwtProvider jwtProvider,
        CancellationToken cancellationToken
    )
    {
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        var newUser = new User()
        {
            Email = request.Email,
            UserName = request.Username,
            Bio = request.Bio,
            IsPrivate = request.IsPrivate,
            CreatedAtUtc = DateTime.UtcNow,
        };

        var result = await userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded)
            return TypedResults.BadRequest(result.Errors);

        var accessToken = await jwtProvider.GenerateJwt(newUser);
        var response = new Response(accessToken);

        await transaction.CommitAsync(cancellationToken);
        return TypedResults.Ok(response);
    }
}
