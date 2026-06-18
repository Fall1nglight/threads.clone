using Microsoft.EntityFrameworkCore;
using Threads.Api.Data.Posts;
using Threads.Api.Data.Users;

namespace Threads.Api.Data.Shared;

public sealed class AppDbContext : DbContext
{
    private const string DefaultSchema = "ThreadsApp";

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // we use the assembly of AppDbContext because it is in the same project as the entity configuration
        //  and it is considered to be best practice
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // applies a so called schema (prefix) to the context
        //  enables multiple applications to use the same database
        modelBuilder.HasDefaultSchema(DefaultSchema);

        // passing modelBuilder to the base class
        base.OnModelCreating(modelBuilder);
    }
}
