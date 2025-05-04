namespace LibraryManagement.Core.Application.DTOs.Responses
{
    public class BorrowResponse
    {
        public Guid Id { get; set; }

        public string RequestorID { get; set; }

        public string RequestorName { get; set; }

        public DateTime BorrowDate { get; set; }

        public string ApproverID { get; set; }

        public string ApproverName { get; set; }

        public RequestStatus Status { get; set; }

        // public DateTime ReturnDate { get; set; }

        public List<BookBorrowingRequestDetailsResponse> Details { get; set; }
    }
}
