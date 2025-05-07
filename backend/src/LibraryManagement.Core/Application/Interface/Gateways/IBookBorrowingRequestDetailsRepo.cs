using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBookBorrowingRequestDetailsRepo : IBaseRepo<BookBorrowingRequestDetails>
    {
        Task UpdateRequestDetailStatusAsync(Guid requestId, BorrowBookStatus status);

        Task<ICollection<BookBorrowingRequestDetails>> GetRequestDetailByRequestId(Guid requestId);
    }
}
