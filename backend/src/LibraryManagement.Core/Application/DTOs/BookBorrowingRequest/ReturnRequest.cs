namespace LibraryManagement.Core.Application.DTOs.Requests
{
    public class ReturnRequest
    {
        public Guid RequestId { get; set; }

        public List<Guid> BookIds { get; set; } = new();
    }
}
