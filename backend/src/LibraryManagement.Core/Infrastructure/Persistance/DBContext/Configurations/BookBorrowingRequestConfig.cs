using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Core.Infrastructure.Persistence.DBContext.Configurations
{
    public class BookBorrowingRequestConfig : IEntityTypeConfiguration<BookBorrowingRequest>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequest> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.DateRequested).IsRequired();

            builder.Property(e => e.Status).IsRequired();

            builder
                .HasOne(e => e.Requestor)
                .WithMany(e => e.BorrowingRequests)
                .HasForeignKey(e => e.RequestorId);

            builder
                .HasOne(e => e.Approver)
                .WithMany(e => e.RequestsToApprove)
                .HasForeignKey(e => e.ApproverId);
        }
    }
}
