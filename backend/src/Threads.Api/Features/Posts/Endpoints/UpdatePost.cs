using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threads.Api.Common.Extensions;
using Threads.Api.Data.Shared;
using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Features.Posts.Endpoints;

public class UpdatePost : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPut("/{id}", Handle).WithValidation<Request>().WithSummary("Updates a post");
    }

    public record Request(Guid Id, [FromBody] Body Body);

    public record Body(string Content);

    public class UpdatePostValidator : AbstractValidator<Request>
    {
        public UpdatePostValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Body.Content).NotEmpty().MaximumLength(600);
        }
    }

    private static async Task<Results<NoContent, NotFound, ForbidHttpResult>> Handle(
        [AsParameters] Request request,
        ClaimsPrincipal claimsPrincipal,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (post == null)
            return TypedResults.NotFound();

        var userId = claimsPrincipal.GetUserId();
        if (post.UserId != userId)
            return TypedResults.Forbid();

        post.Content = request.Body.Content;
        post.UpdatedAtUtc = DateTime.UtcNow;

        await db.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
