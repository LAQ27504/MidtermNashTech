using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Application.Service;
using LibraryManagement.Core.Domains.Entities;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test.Services
{
    [TestFixture]
    public class BookServiceTests
    {
        private Mock<IBookRepo> _bookRepoMock;
        private Mock<ICategoryRepo> _categoryRepoMock;
        private Mock<IUnitOfWork> _uowMock;
        private BookService _service;
        private Guid _categoryId;
        private Book _existing;

        [SetUp]
        public void SetUp()
        {
            _categoryId = Guid.NewGuid();
            _existing = new Book
            {
                Id = Guid.NewGuid(),
                Name = "Existing",
                Author = "Auth",
                CategoryId = _categoryId,
                Amount = 5,
                AvailableAmount = 5,
            };

            _bookRepoMock = new Mock<IBookRepo>();
            _categoryRepoMock = new Mock<ICategoryRepo>();
            _uowMock = new Mock<IUnitOfWork>();

            // Defaults for UoW
            _uowMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _uowMock.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);
            _uowMock.Setup(x => x.RollbackAsync()).Returns(Task.CompletedTask);
            _uowMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Book repo stubs
            _bookRepoMock
                .Setup(x => x.GetBookByNameAndAuthor(_existing.Name, _existing.Author))
                .ReturnsAsync(_existing);
            _bookRepoMock.Setup(x => x.GetByIdAsync(_existing.Id)).ReturnsAsync(_existing);
            _bookRepoMock
                .Setup(x => x.GetBookResponseByIdAsync(_existing.Id))
                .ReturnsAsync(
                    new BookResponse
                    {
                        Id = _existing.Id,
                        Name = _existing.Name,
                        Author = _existing.Author,
                        CategoryId = _existing.CategoryId,
                        CategoryName = "Cat",
                        Amount = _existing.Amount,
                        AvailableAmount = _existing.AvailableAmount,
                    }
                );
            _bookRepoMock
                .Setup(x => x.GetAllBooksAsync())
                .ReturnsAsync(new List<BookResponse> { new() { Id = _existing.Id } });
            _bookRepoMock
                .Setup(x => x.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int p, int s) => (new List<Book> { _existing }, 1));

            // Category repo stubs
            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(_categoryId))
                .ReturnsAsync(new Category { Id = _categoryId, Name = "Cat" });
            _categoryRepoMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(
                    new List<Category>
                    {
                        new() { Id = _categoryId, Name = "Cat" },
                    }
                );

            _service = new BookService(
                _bookRepoMock.Object,
                _categoryRepoMock.Object,
                _uowMock.Object
            );
        }

        [Test]
        public async Task CreateBook_New_Succeeds()
        {
            var req = new BookRequest
            {
                Name = "New",
                Author = "A",
                Amount = 3,
                CategoryId = _categoryId,
            };
            _bookRepoMock.Setup(x => x.GetBookByNameAndAuthor("New", "A")).ReturnsAsync((Book)null);

            var res = await _service.CreateBookExecute(req);

            Assert.IsTrue(res.Success);
            _bookRepoMock.Verify(x => x.AddAsync(It.Is<Book>(b => b.Name == "New")), Times.Once);
            _uowMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task CreateBook_Duplicate_Fails()
        {
            var req = new BookRequest
            {
                Name = "Existing",
                Author = "Auth",
                Amount = 1,
                CategoryId = _categoryId,
            };
            var res = await _service.CreateBookExecute(req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("A book with the same name and author already exists.", res.Message);
        }

        [Test]
        public async Task GetById_Exists_ReturnsData()
        {
            var res = await _service.GetBookByIdExecute(_existing.Id);
            Assert.IsTrue(res.Success);
            Assert.IsInstanceOf<BookResponse>(res.Data);
        }

        [Test]
        public async Task GetById_Missing_Fails()
        {
            _bookRepoMock
                .Setup(x => x.GetBookResponseByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((BookResponse)null);

            var res = await _service.GetBookByIdExecute(Guid.NewGuid());
            Assert.IsFalse(res.Success);
            Assert.AreEqual("Book not found.", res.Message);
        }

        [Test]
        public async Task GetAll_ReturnsList()
        {
            var res = await _service.GetAllBooksExecute();
            Assert.IsTrue(res.Success);
            Assert.IsInstanceOf<IEnumerable<BookResponse>>(res.Data);
        }

        [Test]
        public async Task GetPaged_Invalid_Fails()
        {
            var res = await _service.GetBookPagedExecute(0, 0);
            Assert.IsFalse(res.Success);
            Assert.AreEqual("Page number and size must be greater than zero.", res.Message);
        }

        [Test]
        public async Task GetPaged_Valid_ReturnsPage()
        {
            var res = await _service.GetBookPagedExecute(1, 1);
            Assert.IsTrue(res.Success);
            var page = (PaginationResponse<BookResponse>)res.Data;
            Assert.AreEqual(1, page.Items.Count);
            Assert.AreEqual(1, page.TotalCount);
        }

        [Test]
        public async Task Update_Missing_Fails()
        {
            _bookRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Book)null);

            var req = new BookRequest
            {
                Name = "X",
                Author = "Y",
                Amount = 1,
                CategoryId = _categoryId,
            };
            var res = await _service.UpdateBookExecute(Guid.NewGuid(), req);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("Book not found.", res.Message);
        }

        [Test]
        public async Task Update_Existing_Succeeds()
        {
            var req = new BookRequest
            {
                Name = "Upd",
                Author = "A",
                Amount = 10,
                CategoryId = _categoryId,
            };
            var res = await _service.UpdateBookExecute(_existing.Id, req);

            Assert.IsTrue(res.Success);
            _bookRepoMock.Verify(
                x => x.Update(It.Is<Book>(b => b.Name == "Upd" && b.Amount == 10)),
                Times.Once
            );
            _uowMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task Delete_Existing_Succeeds()
        {
            var res = await _service.DeleteBookExecute(_existing.Id);

            Assert.IsTrue(res.Success);
            _bookRepoMock.Verify(x => x.Delete(It.Is<Book>(b => b.Id == _existing.Id)), Times.Once);
        }

        [Test]
        public async Task AddMulti_MissingCategory_Fails()
        {
            _categoryRepoMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category)null);

            var reqs = new[]
            {
                new BookRequest
                {
                    Name = "X",
                    Author = "Y",
                    Amount = 1,
                    CategoryId = Guid.NewGuid(),
                },
            };
            var res = await _service.AddMultiBooksExecute(reqs);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("One or more categories not found.", res.Message);
        }

        [Test]
        public async Task AddMulti_Valid_Succeeds()
        {
            var reqs = new[]
            {
                new BookRequest
                {
                    Name = "B1",
                    Author = "A",
                    Amount = 1,
                    CategoryId = _categoryId,
                },
                new BookRequest
                {
                    Name = "B2",
                    Author = "A",
                    Amount = 2,
                    CategoryId = _categoryId,
                },
            };
            var res = await _service.AddMultiBooksExecute(reqs);

            Assert.IsTrue(res.Success);
            _bookRepoMock.Verify(
                x => x.AddRangeAsync(It.Is<ICollection<Book>>(c => c.Count == 2)),
                Times.Once
            );
            _uowMock.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task Decrease_NotEnough_ReturnsFalse()
        {
            var ok = await _service.DecreaseAvailableAmount(_existing.Id, 10);
            Assert.IsFalse(ok);
        }

        [Test]
        public async Task Decrease_Enough_ReturnsTrue()
        {
            var ok = await _service.DecreaseAvailableAmount(_existing.Id, 2);
            Assert.IsTrue(ok);
            Assert.AreEqual(3, _existing.AvailableAmount);
        }

        [Test]
        public async Task Increase_Works()
        {
            await _service.IncreaseAvailableAmount(_existing.Id, 2);
            Assert.AreEqual(7, _existing.AvailableAmount);
        }
    }
}
