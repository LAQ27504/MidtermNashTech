using LibraryManagement.API.Infrastructure.Persistence.Seed;
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
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            if (!builder.Metadata.GetSeedData().Any())
            {
                builder.HasData(BookBorrowingRequestSeeding.SeedBookRequests());
            }
        }
    }
}
