using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Core.Infrastructure.Persistence.DBContext
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);

            builder.Property(e => e.HashedPassword).IsRequired();

            builder.Property(e => e.Type).IsRequired();

            throw new NotImplementedException();
        }
    }
}
