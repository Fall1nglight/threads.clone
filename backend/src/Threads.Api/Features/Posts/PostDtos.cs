namespace Threads.Api.Features.Posts;

public record PostDto()
{
    public required Guid Id { get; init; }
    public required UserDto User { get; init; }
    public required string Content { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
};

public record UserDto()
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
};
