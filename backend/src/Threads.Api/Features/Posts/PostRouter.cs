using Threads.Api.Common.Extensions;
using Threads.Api.Data.Shared.Interfaces;
using Threads.Api.Features.Posts.Endpoints;

namespace Threads.Api.Features.Posts;

public class PostRouter : IEndpointRouter
{
    public static void MapRouter(IEndpointRouteBuilder builder)
    {
        builder
            .MapGroup("/posts")
            .RequireAuthorization()
            .MapEndpoint<GetPosts>()
            .MapEndpoint<GetPost>()
            .MapEndpoint<CreatePost>()
            .MapEndpoint<UpdatePost>()
            .MapEndpoint<DeletePost>();
    }
}
