using LibraryManagement.Core.Application.DTOs.Requests;

namespace LibraryManagement.Core.Application.Interface.Services
{
    public interface IBookBorrowingRequestService
    {
        Task<OperationResult> CreateBorrowRequestAsync(BorrowRequest request);
        Task<OperationResult> GetBorrowRequestByIdAsync(Guid requestId);
        Task<OperationResult> GetRequestsByUserAsync(Guid requestorId);
        Task<OperationResult> ReturnBooksAsync(ReturnRequest request);
        Task<OperationResult> SetApprovalAsync(Guid requestId, Guid approverId, bool isApproved);
    }
}
