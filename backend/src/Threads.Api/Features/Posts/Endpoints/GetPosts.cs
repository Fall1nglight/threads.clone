using Microsoft.AspNetCore.Http.HttpResults;
using Threads.Api.Common.Pagination;
using Threads.Api.Data.Shared;
using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Features.Posts.Endpoints;

public class GetPosts : IEndpoint
{
    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", Handle).WithSummary("Retrieves posts from public accounts");
    }

    private static async Task<Ok<PagedResponse<PostDto>>> Handle(
        [AsParameters] PagedRequest request,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var posts = await db
            .Posts.Where(p => !p.User.IsPrivate)
            .ToDto()
            .ToPagedResponse(request, cancellationToken);

        return TypedResults.Ok(posts);
    }
}
