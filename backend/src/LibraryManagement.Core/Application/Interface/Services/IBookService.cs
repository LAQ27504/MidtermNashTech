using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Service;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Interface.Services
{
    public interface IBookService
    {
        Task<OperationResult> CreateBookExecute(BookRequest book);
        Task<OperationResult> AddMultiBooksExecute(ICollection<BookRequest> requests);
        Task<OperationResult> GetBookByIdExecute(Guid id);
        Task<OperationResult> GetAllBooksExecute();
        Task<OperationResult> UpdateBookExecute(Guid id, BookRequest book);
        Task<OperationResult> DeleteBookExecute(Guid id);
        Task<OperationResult> GetBookPagedExecute(int pageNumber, int pageSize);
        Task<bool> DecreaseAvailableAmount(Guid bookId, int quantity);
        Task IncreaseAvailableAmount(Guid bookId, int quantity);
    }
}
