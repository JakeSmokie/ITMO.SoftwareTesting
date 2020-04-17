using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITMO.SoftwareTesting.Dates.Database.Configuration
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.Purpose)
                .IsRequired();

            builder
                .HasMany(x => x.Users)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId);

            builder
                .HasOne(x => x.OwnerUser)
                .WithMany()
                .HasForeignKey(x => x.OwnerUserId);

            builder
                .HasMany(x => x.Invitations)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId);
        }
    }
}