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

            builder.HasData(
                new BookBorrowingRequestDetails
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    BookId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Book: "Introduction to Algorithms"
                    RequestId = Guid.Parse("77777777-7777-7777-7777-777777777777"), // Request: BorrowingRequest
                }
            );
        }
    }
}
