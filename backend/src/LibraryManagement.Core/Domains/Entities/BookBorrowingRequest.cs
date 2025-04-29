namespace LibraryManagement.Core.Domains.Entities
{
    public class BookBorrowingRequest
    {
        public Guid Id { get; set; }

        public DateTime DateRequested { get; set; }

        public RequestStatus Status { get; set; }

        public int RequestorId { get; set; }

        public required User Requestor { get; set; }

        public int? ApproverId { get; set; }

        public User? Approver { get; set; }

        public required ICollection<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }
    }
}
