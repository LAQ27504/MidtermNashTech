namespace LibraryManagement.Core.Application.DTOs.Requests
{
    public class BorrowRequest
    {
        public Guid RequestorID { get; set; }

        public List<Guid> BookIds { get; set; }
    }
}
