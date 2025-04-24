namespace LibraryManagement.Core.Domains.Entities
{
    public class BookBorrowingRequestDetails
    {
        public int Id { get; set; }

        public ICollection<Book> BorrowedBooks { get; set; }

        public int BookBorrowingRequestId { get; set; }

        public BookBorrowingRequest BookBorrowingRequest { get; set; }
    }
}
