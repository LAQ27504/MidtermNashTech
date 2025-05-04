namespace LibraryManagement.Core.Application.DTOs.Responses
{
    public class BookBorrowingRequestDetailsResponse
    {
        public Guid Id { get; set; } // Detail ID
        public Guid BookId { get; set; }
        public string BookName { get; set; } = string.Empty; // Added for display
        public string BookAuthor { get; set; } = string.Empty; // Added for display
        public BorrowBookStatus Status { get; set; }
    }
}
