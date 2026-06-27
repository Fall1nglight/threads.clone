using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Threads.Api.Data.Shared;
using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Features.Posts.Endpoints;

public class GetPosts : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", Handle).WithSummary("Retrieves posts from public accounts");
    }

    private static async Task<Ok<List<PostDto>>> Handle(
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var posts = await db
            .Posts.Where(p => !p.User.IsPrivate)
            .ProjectToResponse()
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(posts);
    }
}
