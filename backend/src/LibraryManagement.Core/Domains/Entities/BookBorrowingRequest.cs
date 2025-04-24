namespace LibraryManagement.Core.Domains.Entities
{
    public class BookBorrowingRequest
    {
        public int Id { get; set; }

        public int RequestorId { get; set; }

        public User Requestor { get; set; }

        public DateTime DateRequested { get; set; }

        public RequestStatus Status { get; set; }

        public int ApproverId { get; set; }

        public User Approver { get; set; }

        public BookBorrowingRequestDetails BookBorrowingRequestDetails { get; set; }
    }
}
