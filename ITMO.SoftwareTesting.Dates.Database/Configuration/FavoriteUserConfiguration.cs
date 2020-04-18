using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITMO.SoftwareTesting.Dates.Database.Configuration
{
    public class FavoriteUserConfiguration : IEntityTypeConfiguration<FavoriteUser>
    {
        public void Configure(EntityTypeBuilder<FavoriteUser> builder)
        {
            builder.HasKey(x => new {x.FirstUserId, x.SecondUserId});

            builder
                .HasOne(x => x.First)
                .WithMany(x => x.FavoriteUsers)
                .HasForeignKey(x => x.FirstUserId);

            builder
                .HasOne(x => x.Second)
                .WithMany()
                .HasForeignKey(x => x.SecondUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}