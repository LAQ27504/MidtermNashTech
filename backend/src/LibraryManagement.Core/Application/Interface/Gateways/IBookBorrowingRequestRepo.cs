using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBookBorrowingRequestRepo : IBaseRepo<BookBorrowingRequest>
    {
        public Task<ICollection<BookBorrowingRequest>> GetWaitingRequests();
        public Task<List<BookBorrowingRequest>> GetRequestsWithDetailsByUserAsync(
            Guid userequestorId
        );

        public Task<int> GetNumOfRequestThisMonth(Guid requestorId);
        public Task<ICollection<BorrowResponse>> GetBorrowResponsesByRequestIdAsync(
            Guid requestorID
        );
    }
}
