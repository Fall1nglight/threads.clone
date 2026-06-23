using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Threads.Api.Data.Follows;
using Threads.Api.Data.Posts;
using Threads.Api.Data.Users;

namespace Threads.Api.Data.Shared;

public sealed class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Follow> Follows { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // passing builder to the base class
        base.OnModelCreating(builder);

        // we use the assembly of AppDbContext because it is in the same project as the entity configuration
        //  and it is considered to be best practice
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // applies a so called schema (prefix) to the context
        //  enables multiple applications to use the same database
        builder.HasDefaultSchema(DbContextSchemas.Default);

        ConfigureIdentity(builder);
    }

    /// <summary>
    /// Configures the Identity framework entity mappings for the specified <see cref="ModelBuilder"/>.
    /// This includes customizing table names and schemas for Identity entities such as users, roles, claims, logins, and tokens.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ModelBuilder"/> used to configure the entity mappings and schema settings for Identity framework entities.
    /// </param>
    private void ConfigureIdentity(ModelBuilder builder)
    {
        builder.Entity<User>().ToTable(DbContextTableNames.Users, DbContextSchemas.Identity);

        builder
            .Entity<IdentityRole<Guid>>()
            .ToTable(DbContextTableNames.Roles, DbContextSchemas.Identity);

        builder
            .Entity<IdentityUserRole<Guid>>()
            .ToTable(DbContextTableNames.UserRoles, DbContextSchemas.Identity);

        builder
            .Entity<IdentityUserClaim<Guid>>()
            .ToTable(DbContextTableNames.UserClaims, DbContextSchemas.Identity);

        builder
            .Entity<IdentityUserLogin<Guid>>()
            .ToTable(DbContextTableNames.UserLogins, DbContextSchemas.Identity);

        builder
            .Entity<IdentityRoleClaim<Guid>>()
            .ToTable(DbContextTableNames.RoleClaims, DbContextSchemas.Identity);

        builder
            .Entity<IdentityUserToken<Guid>>()
            .ToTable(DbContextTableNames.UserTokens, DbContextSchemas.Identity);
    }
}
