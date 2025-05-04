using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IBookRepo bookRepo, ICategoryRepo categoryRepo, IUnitOfWork unitOfWork)
        {
            _bookRepo = bookRepo;
            _categoryRepo = categoryRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> CreateBookExecute(BookRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Check for duplicate book by name and author
                var exists = await _bookRepo.GetBookByNameAndAuthor(request.Name, request.Author);
                if (exists != null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail(
                        "A book with the same name and author already exists."
                    );
                }

                var category = await _categoryRepo.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("Category not found.");
                }

                var entity = new Book
                {
                    Name = request.Name,
                    Author = request.Author,
                    Amount = request.Amount,
                    CategoryId = request.CategoryId,
                    AvailableAmount = request.Amount,
                };

                await _bookRepo.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return OperationResult.Ok("Book created successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to create book: {ex.Message}");
            }
        }

        public async Task<OperationResult> GetBookByIdExecute(Guid id)
        {
            var entity = await _bookRepo.GetBookResponseByIdAsync(id);
            if (entity == null)
                return OperationResult.Fail("Book not found.");

            return OperationResult.Ok(entity, "Book retrieved successfully.");
        }

        public async Task<OperationResult> GetAllBooksExecute()
        {
            var res = await _bookRepo.GetAllBooksAsync();

            return OperationResult.Ok(res, "Books retrieved successfully.");
        }

        public async Task<OperationResult> GetBookPagedExecute(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return OperationResult.Fail("Page number and size must be greater than zero.");

            var (entities, total) = await _bookRepo.GetPagedAsync(pageNumber, pageSize);
            var categories = await _categoryRepo.GetAllAsync();

            var responses = entities.Select(b => new BookResponse
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Amount = b.Amount,
                AvailableAmount = b.AvailableAmount,
                CategoryName = categories.FirstOrDefault(c => c.Id == b.CategoryId)!.Name,
            });

            var pagedResponse = new PaginationResponse<BookResponse>
            {
                Items = responses.ToList(),
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            return OperationResult.Ok(pagedResponse, "Books retrieved successfully.");
        }

        public async Task<OperationResult> UpdateBookExecute(Guid id, BookRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = await _bookRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("Book not found.");
                }

                var category = await _categoryRepo.GetByIdAsync(request.CategoryId);
                if (category == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("Category not found.");
                }

                entity.Name = request.Name;
                entity.Author = request.Author;
                entity.Amount = request.Amount;
                entity.CategoryId = request.CategoryId;
                entity.AvailableAmount = entity.AvailableAmount + (request.Amount - entity.Amount);

                _bookRepo.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return OperationResult.Ok("Book updated successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to update book: {ex.Message}");
            }
        }

        public async Task<OperationResult> DeleteBookExecute(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = await _bookRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("Book not found.");
                }

                _bookRepo.Delete(entity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return OperationResult.Ok("Book deleted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to delete book: {ex.Message}");
            }
        }

        public async Task<OperationResult> AddMultiBooksExecute(ICollection<BookRequest> requests)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var categoryIds = requests.Select(r => r.CategoryId).Distinct();
                var categories = new List<Category>();

                foreach (var categoryId in categoryIds)
                {
                    var category = await _categoryRepo.GetByIdAsync(categoryId);
                    if (category != null)
                    {
                        categories.Add(category);
                    }
                }

                if (categories.Count() != categoryIds.Count())
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("One or more categories not found.");
                }

                var books = new List<Book>();

                foreach (var request in requests)
                {
                    // Check for duplicate book by name and author
                    var exists = await _bookRepo.GetBookByNameAndAuthor(
                        request.Name,
                        request.Author
                    );
                    if (exists != null)
                    {
                        await _unitOfWork.RollbackAsync();
                        return OperationResult.Fail(
                            "A book with the same name and author already exists."
                        );
                    }

                    var entity = new Book
                    {
                        Name = request.Name,
                        Author = request.Author,
                        Amount = request.Amount,
                        CategoryId = request.CategoryId,
                    };
                    books.Add(entity);
                }

                await _bookRepo.AddRangeAsync(books);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return OperationResult.Ok("Books created successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail($"Failed to create books: {ex.Message}");
            }
        }

        public async Task<bool> DecreaseAvailableAmount(Guid bookId, int quantity)
        {
            var book = await _bookRepo.GetByIdAsync(bookId);
            if (book == null || book.AvailableAmount < quantity)
                return false;

            book.AvailableAmount -= quantity;
            _bookRepo.Update(book);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task IncreaseAvailableAmount(Guid bookId, int quantity)
        {
            var book = await _bookRepo.GetByIdAsync(bookId);
            if (book != null)
            {
                book.AvailableAmount += quantity;
                _bookRepo.Update(book);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
