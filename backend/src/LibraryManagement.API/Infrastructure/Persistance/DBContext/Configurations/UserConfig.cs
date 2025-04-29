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

            builder.HasData(
                new User
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "John Doe",
                    Type = UserType.NormalUser,
                    HashedPassword = "hashed_password_1",
                },
                new User
                {
                    Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    Name = "Jane Smith",
                    Type = UserType.SuperUser,
                    HashedPassword = "hashed_password_2",
                }
            );
        }
    }
}
