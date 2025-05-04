using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBookRepo : IBaseRepo<Book>
    {
        public Task<Book?> GetBookByNameAndAuthor(string name, string author);
        public Task<BookResponse?> GetBookResponseByIdAsync(Guid id);

        public Task<ICollection<BookResponse>> GetAllBooksAsync();
    }
}
