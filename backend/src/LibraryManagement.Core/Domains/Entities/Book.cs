namespace LibraryManagement.Core.Domains.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int Amount { get; set; }

        public Category BookCategory { get; set; }

        public ICollection<BookBorrowingRequestDetails> RequestDetails { get; set; }
    }
}
