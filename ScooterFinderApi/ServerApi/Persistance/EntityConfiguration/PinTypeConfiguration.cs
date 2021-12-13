using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerApi.Persistance.Models;

namespace ServerApi.Persistance.EntityConfiguration
{
    public class PinTypeConfiguration : IEntityTypeConfiguration<PinTypes>
    {
        public void Configure(EntityTypeBuilder<PinTypes> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(256);
        }
    }
}
