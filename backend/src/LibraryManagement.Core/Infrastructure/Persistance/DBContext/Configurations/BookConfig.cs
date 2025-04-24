using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Core.Infrastructure.Persistence.DBContext
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            builder.Property(e => e.Amount).IsRequired();

            builder
                .HasOne(e => e.BookCategory)
                .WithMany(e => e.Books)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            throw new NotImplementedException();
        }
    }
}
