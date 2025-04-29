using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.API.Infrastructure.Persistence.DBContext.Configurations
{
    public class BookBorrowingRequestConfig : IEntityTypeConfiguration<BookBorrowingRequest>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequest> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.DateRequested).IsRequired();

            builder.Property(r => r.Status).IsRequired();

            builder
                .HasOne(r => r.Requestor)
                .WithMany(u => u.BorrowingRequests)
                .HasForeignKey(r => r.RequestorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(r => r.Approver)
                .WithMany(u => u.RequestsToApprove)
                .HasForeignKey(r => r.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new BookBorrowingRequest
                {
                    Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    DateRequested = new DateTime(2025, 4, 28),
                    Status = RequestStatus.Approved,
                    RequestorId = Guid.Parse("55555555-5555-5555-5555-555555555555"), // Again, assuming mapping manually if needed
                    ApproverId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                }
            );
        }
    }
}
