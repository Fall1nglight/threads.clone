using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Threads.Api.Data.Follows;

public class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.HasKey(x => new { x.FollowerId, x.FollowedId });

        // enables to store Enums as strings in the database
        //  which improves readability
        builder.Property(x => x.Status).HasConversion<string>();

        // restrict delete behaviour prevents the deletion of the parent entity
        //  if related children exist
        builder
            .HasOne(x => x.Follower)
            .WithMany()
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Followed)
            .WithMany()
            .HasForeignKey(x => x.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
