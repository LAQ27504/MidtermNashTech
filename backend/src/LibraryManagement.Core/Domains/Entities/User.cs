namespace LibraryManagement.Core.Domains.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public UserType Type { get; set; }

        public required string HashedPassword { get; set; }

        public required ICollection<BookBorrowingRequest> BorrowingRequests { get; set; }

        public required ICollection<BookBorrowingRequest> RequestsToApprove { get; set; }
    }
}
