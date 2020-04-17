using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITMO.SoftwareTesting.Dates.Database.Configuration
{
    public class FavoriteEventConfiguration : IEntityTypeConfiguration<FavoriteEvent>
    {
        public void Configure(EntityTypeBuilder<FavoriteEvent> builder)
        {
            builder.HasKey(x => new {x.EventId, x.UserId});

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.FavoriteEvents)
                .HasForeignKey(x => x.UserId);
        }
    }
}