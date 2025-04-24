namespace LibraryManagement.Core.Domains.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public UserType Type { get; set; }

        public string HashedPassword { get; set; }

        public ICollection<BookBorrowingRequest> BorrowingRequests { get; set; }

        public ICollection<BookBorrowingRequest> RequestsToApprove { get; set; }
    }
}
