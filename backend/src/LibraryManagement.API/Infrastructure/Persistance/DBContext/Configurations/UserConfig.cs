using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.API.Infrastructure.Persistence.DBContext
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name).IsRequired().HasMaxLength(50).IsUnicode();

            builder.HasIndex(u => u.Name).IsUnique();

            builder.Property(u => u.HashedPassword).IsRequired();

            builder.Property(u => u.Type).IsRequired();
        }
    }
}
