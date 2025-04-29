namespace LibraryManagement.Core.Domains.Entities
{
    public class BookBorrowingRequestDetails
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public required Book BorrowBook { get; set; }

        public Guid RequestId { get; set; }

        public required BookBorrowingRequest BookBorrowingRequest { get; set; }
    }
}
