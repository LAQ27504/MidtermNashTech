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

            builder
                .HasOne(b => b.BookCategory)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Book
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Introduction to Algorithms",
                    CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Assuming you map Category manually since your CategoryId is int
                    Amount = 10,
                },
                new Book
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Hamlet",
                    CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Amount = 5,
                }
            );
        }
    }
}
