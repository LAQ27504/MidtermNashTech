using LibraryManagement.API.Infrastructure.Persistence.Seed;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.API.Infrastructure.Persistence.DBContext
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name).IsRequired().HasMaxLength(100);

            builder.Property(b => b.Amount).IsRequired();

            builder.Property(b => b.AvailableAmount).IsRequired();

            builder.Property(b => b.Author).IsRequired().HasMaxLength(100);

            builder
                .HasOne(b => b.BookCategory)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            if (!builder.Metadata.GetSeedData().Any())
            {
                builder.HasData(BookSeeding.SeedBooks());
            }
        }
    }
}
