using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerApi.Persistance.Models;

namespace ServerApi.Persistance.EntityConfiguration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).HasMaxLength(2048);
            builder.Property(x => x.Date);
            builder.HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Pin)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.PinId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
