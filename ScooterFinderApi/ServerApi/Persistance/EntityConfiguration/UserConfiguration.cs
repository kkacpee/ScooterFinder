using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerApi.Persistance.Models;

namespace ServerApi.Persistance.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).HasMaxLength(256);
            builder.Property(x => x.DisplayName).HasMaxLength(256);
            builder.Property(x => x.PasswordSalt).HasMaxLength(1024);
            builder.Property(x => x.PasswordHash).HasMaxLength(256);
        }
    }
}
