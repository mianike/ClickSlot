using ClickSlotDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClickSlotDAL.EntityConfigurations
{
    internal class MasterConfiguration : IEntityTypeConfiguration<Master>
    {
        public void Configure(EntityTypeBuilder<Master> builder)
        {
        }
    }
}
