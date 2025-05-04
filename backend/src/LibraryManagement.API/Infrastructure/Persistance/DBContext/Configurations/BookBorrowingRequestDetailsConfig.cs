using LibraryManagement.API.Infrastructure.Persistence.Seed;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.API.Infrastructure.Persistence.DBContext.Configurations
{
    public class BookBorrowingRequestDetailsConfig
        : IEntityTypeConfiguration<BookBorrowingRequestDetails>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequestDetails> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Status).IsRequired();

            builder
                .HasOne(d => d.BorrowBook)
                .WithMany(b => b.RequestDetails)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(d => d.BookBorrowingRequest)
                .WithMany(r => r.BookBorrowingRequestDetails)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
            if (!builder.Metadata.GetSeedData().Any())
            {
                builder.HasData(BookBorrowingRequestDetailsSeeding.SeedBookRequestDetails());
            }
        }
    }
}
