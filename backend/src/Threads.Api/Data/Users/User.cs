using Microsoft.AspNetCore.Identity;
using Threads.Api.Data.Posts;
using Threads.Api.Data.Shared.Interfaces;

namespace Threads.Api.Data.Users;

public class User : IdentityUser<Guid>
{
    public required string Username { get; set; }
    public required string HashedPassword { get; set; }
    public string? Bio { get; set; }
    public bool IsPrivate { get; set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; set; }

    public List<Post> Posts { get; set; } = [];
}
