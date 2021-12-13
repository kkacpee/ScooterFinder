using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerApi.Persistance.Models;

namespace ServerApi.Persistance.EntityConfiguration
{
    public class PinConfiguration : IEntityTypeConfiguration<Pin>
    {
        public void Configure(EntityTypeBuilder<Pin> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PinName).HasMaxLength(256);
            builder.Property(x => x.Coordinates).HasMaxLength(256);
            builder.Property(x => x.Description).HasMaxLength(2056);
            builder.Property(x => x.CreationDate);
            builder.HasOne(x => x.User)
                .WithMany(x => x.Pins)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.PinType)
                .WithMany(x => x.Pins)
                .HasForeignKey(x => x.PinTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
