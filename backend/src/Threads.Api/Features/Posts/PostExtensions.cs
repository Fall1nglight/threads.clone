using Threads.Api.Data.Posts;
using Threads.Api.Data.Users;

namespace Threads.Api.Features.Posts;

public static class PostExtensions
{
    public static IQueryable<PostDto> ProjectToResponse(this IQueryable<Post> query)
    {
        return query.Select(post => new PostDto
        {
            Id = post.Id,
            User = new UserDto { Id = post.User.Id, Username = post.User.UserName! },
            Content = post.Content,
            CreatedAtUtc = post.CreatedAtUtc,
            UpdatedAtUtc = post.UpdatedAtUtc,
        });
    }

    public static PostDto ToResponse(this Post post) => post.ToResponse(post.User);

    public static PostDto ToResponse(this Post post, User user)
    {
        return new PostDto
        {
            Id = post.Id,
            User = user.ToDto(),
            Content = post.Content,
            CreatedAtUtc = post.CreatedAtUtc,
            UpdatedAtUtc = post.UpdatedAtUtc,
        };
    }

    public static UserDto ToDto(this User user)
    {
        return new UserDto { Id = user.Id, Username = user.UserName! };
    }
}
