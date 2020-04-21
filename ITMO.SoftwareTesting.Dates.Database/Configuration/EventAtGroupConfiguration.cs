using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITMO.SoftwareTesting.Dates.Database.Configuration
{
    public class EventAtGroupConfiguration : IEntityTypeConfiguration<EventAtGroup>
    {
        public void Configure(EntityTypeBuilder<EventAtGroup> builder)
        {
            builder.HasKey(x => new {x.EventId, x.GroupId});

            builder
                .HasOne(x => x.Group)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.GroupId);

            
        }
    }
}