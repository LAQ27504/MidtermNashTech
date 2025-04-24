using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Core.Infrastructure.Persistence.DBContext.Configurations
{
    public class BookBorrowingRequestDetailsConfig
        : IEntityTypeConfiguration<BookBorrowingRequestDetails>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequestDetails> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(e => e.BorrowedBooks).WithMany(e => e.RequestDetails);

            builder.HasOne(e => e.BookBorrowingRequest).WithOne(e => e.BookBorrowingRequestDetails);
        }
    }
}
