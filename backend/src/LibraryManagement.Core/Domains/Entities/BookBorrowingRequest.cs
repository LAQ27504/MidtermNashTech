namespace LibraryManagement.Core.Domains.Entities
{
    public class BookBorrowingRequest
    {
        public Guid Id { get; set; }

        public DateTime DateRequested { get; set; }

        public RequestStatus Status { get; set; }

        public Guid RequestorId { get; set; }

        public User Requestor { get; set; }

        public Guid ApproverId { get; set; }

        public User? Approver { get; set; }

        public ICollection<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }
    }
}
