using ClickSlotDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickSlotDAL.EntityConfigurations
{
    internal class ServiceConfiguration : IEntityTypeConfiguration<Offering>
    {
        public void Configure(EntityTypeBuilder<Offering> builder)
        {
        }
    }
}
