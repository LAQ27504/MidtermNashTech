namespace LibraryManagement.Core.Application.DTOs.Requests
{
    public class ApprovalRequest
    {
        public Guid RequestId { get; set; }
        public Guid ApproverId { get; set; }
        public bool IsApproved { get; set; }
    }
}
