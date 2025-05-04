using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class BookRepo : BaseRepo<Book>, IBookRepo
    {
        public BookRepo(ApplicationDBContext context)
            : base(context) { }

        public Task<Book?> GetBookByNameAndAuthor(string name, string author)
        {
            return _context
                .Books.Where(b =>
                    b.Name.ToLower() == name.Trim().ToLower()
                    && b.Author.ToLower() == author.Trim().ToLower()
                )
                .FirstOrDefaultAsync();
        }

        public async Task<BookResponse?> GetBookResponseByIdAsync(Guid id)
        {
            var query = await _dbSet
                .Include(b => b.BookCategory) // <-- eager load the Category
                .Where(b => b.Id == id)
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    Author = b.Author,
                    Amount = b.Amount,
                    CategoryName = b.BookCategory.Name,
                })
                .FirstOrDefaultAsync();
            ;

            if (query == null)
            {
                throw new Exception("Book not found");
            }

            return query;
        }

        public async Task<ICollection<BookResponse>> GetAllBooksAsync()
        {
            var query = await _dbSet
                .Include(b => b.BookCategory) // <-- eager load the Category
                .Select(b => new BookResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    Author = b.Author,
                    Amount = b.Amount,
                    CategoryName = b.BookCategory.Name,
                    AvailableAmount = b.AvailableAmount,
                })
                .ToListAsync();

            if (query == null)
            {
                throw new Exception("Books not found");
            }

            return query;
        }
    }
}
