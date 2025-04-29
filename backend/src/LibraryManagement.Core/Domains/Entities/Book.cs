namespace LibraryManagement.Core.Domains.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int CategoryId { get; set; }

        public int Amount { get; set; }

        public required Category BookCategory { get; set; }

        public required ICollection<BookBorrowingRequestDetails> RequestDetails { get; set; }
    }
}
