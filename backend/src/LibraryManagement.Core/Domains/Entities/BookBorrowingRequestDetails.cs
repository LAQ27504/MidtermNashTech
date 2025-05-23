namespace LibraryManagement.Core.Domains.Entities
{
    public class BookBorrowingRequestDetails
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public Book BorrowBook { get; set; }

        public Guid RequestId { get; set; }

        public BorrowBookStatus Status { get; set; }

        public BookBorrowingRequest BookBorrowingRequest { get; set; }
    }
}
