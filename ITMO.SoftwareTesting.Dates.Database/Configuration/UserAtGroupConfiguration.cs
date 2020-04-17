using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITMO.SoftwareTesting.Dates.Database.Configuration
{
    public class UserAtGroupConfiguration : IEntityTypeConfiguration<UserAtGroup>
    {
        public void Configure(EntityTypeBuilder<UserAtGroup> builder)
        {
            builder.HasKey(x => new {x.GroupId, x.UserId});
        }
    }
}