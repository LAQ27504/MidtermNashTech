namespace LibraryManagement.Core.Domains.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public string Author { get; set; }

        public Guid CategoryId { get; set; }

        public int Amount { get; set; }

        public int AvailableAmount { get; set; }

        public Category BookCategory { get; set; }

        public ICollection<BookBorrowingRequestDetails> RequestDetails { get; set; }
    }
}
