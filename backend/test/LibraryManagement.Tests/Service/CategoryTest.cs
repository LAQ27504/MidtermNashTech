using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Domains.Entities;
using LibraryManagement.Core.Domains.Services;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test.Services
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepo> _catRepo;
        private Mock<IUnitOfWork> _uow;
        private CategoryService _svc;
        private Guid _existingId;
        private Category _existing;

        [SetUp]
        public void Setup()
        {
            _existingId = Guid.NewGuid();
            _existing = new Category { Id = _existingId, Name = "Existing" };

            _catRepo = new Mock<ICategoryRepo>();
            _uow = new Mock<IUnitOfWork>();

            // UoW defaults
            _uow.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.RollbackAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);

            // Repo defaults
            _catRepo.Setup(x => x.GetByNameAsync("Existing")).ReturnsAsync(_existing);
            _catRepo.Setup(x => x.GetByIdAsync(_existingId)).ReturnsAsync(_existing);
            _catRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Category> { _existing });
            _catRepo
                .Setup(x => x.GetPagedAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int p, int s) => (new List<Category> { _existing }, 1));

            _svc = new CategoryService(_uow.Object, _catRepo.Object);
        }

        [Test]
        public async Task CreateCategory_WhenNew_ReturnsOk()
        {
            // Arrange: make GetByNameAsync return null for a new name
            _catRepo.Setup(x => x.GetByNameAsync("New")).ReturnsAsync((Category)null);

            // Act
            var result = await _svc.CreateCategoryExecute(new CategoryRequest { Name = "New" });

            // Assert
            Assert.IsTrue(result.Success);
            _catRepo.Verify(x => x.AddAsync(It.Is<Category>(c => c.Name == "New")), Times.Once);
            _uow.Verify(x => x.CommitAsync(), Times.Once);
            Assert.That(((CategoryResponse)result.Data!).Name, Is.EqualTo("New"));
        }

        [Test]
        public async Task CreateCategory_WhenDuplicate_ReturnsFail()
        {
            var result = await _svc.CreateCategoryExecute(
                new CategoryRequest { Name = "Existing" }
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category already exists", result.Message);
            _uow.Verify(x => x.RollbackAsync(), Times.Once);
        }

        [Test]
        public async Task GetCategoryById_WhenExists_ReturnsOk()
        {
            var result = await _svc.GetCategoryByIdExecute(_existingId);

            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf<CategoryResponse>(result.Data!);
            Assert.AreEqual("Existing", ((CategoryResponse)result.Data!).Name);
        }

        [Test]
        public async Task GetCategoryById_WhenMissing_ReturnsFail()
        {
            _catRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category)null);

            var result = await _svc.GetCategoryByIdExecute(Guid.NewGuid());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category not found", result.Message);
        }

        [Test]
        public async Task GetAllCategories_ReturnsList()
        {
            var result = await _svc.GetAllCategoriesExecute();

            Assert.IsTrue(result.Success);
            var list = (List<CategoryResponse>)result.Data!;
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Existing", list[0].Name);
        }

        [Test]
        public async Task UpdateCategory_WhenMissing_ReturnsFail()
        {
            _catRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category)null);

            var result = await _svc.UpdateCategoryExecute(
                Guid.NewGuid(),
                new CategoryRequest { Name = "X" }
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category not found", result.Message);
            _uow.Verify(x => x.RollbackAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategory_WhenNameTaken_ReturnsFail()
        {
            // Existing other category with same name
            var otherId = Guid.NewGuid();
            _catRepo.Setup(x => x.GetByIdAsync(_existingId)).ReturnsAsync(_existing);
            _catRepo
                .Setup(x => x.GetByNameAsync("Taken"))
                .ReturnsAsync(new Category { Id = otherId, Name = "Taken" });

            var result = await _svc.UpdateCategoryExecute(
                _existingId,
                new CategoryRequest { Name = "Taken" }
            );

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category with this name already exists", result.Message);
            _uow.Verify(x => x.RollbackAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateCategory_WhenValid_UpdatesAndCommits()
        {
            _catRepo.Setup(x => x.GetByNameAsync("NewName")).ReturnsAsync((Category)null);

            var result = await _svc.UpdateCategoryExecute(
                _existingId,
                new CategoryRequest { Name = "NewName" }
            );

            Assert.IsTrue(result.Success);
            Assert.AreEqual("NewName", _existing.Name);
            _uow.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategory_WhenMissing_ReturnsFail()
        {
            _catRepo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Category)null);

            var result = await _svc.DeleteCategoryExecute(Guid.NewGuid());

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category not found", result.Message);
            _uow.Verify(x => x.RollbackAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteCategory_WhenExists_DeletesAndCommits()
        {
            var result = await _svc.DeleteCategoryExecute(_existingId);

            Assert.IsTrue(result.Success);
            _catRepo.Verify(x => x.Delete(_existing), Times.Once);
            _uow.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task GetCategoriesPaged_ReturnsPaged()
        {
            var result = await _svc.GetCategoriesPagedExecute(2, 5);

            Assert.IsTrue(result.Success);
            var page = (PaginationResponse<CategoryResponse>)result.Data!;
            Assert.AreEqual(1, page.Items.Count);
            Assert.AreEqual(1, page.TotalCount);
            Assert.AreEqual(2, page.PageNumber);
            Assert.AreEqual(5, page.PageSize);
        }

        [Test]
        public async Task AddMultiCategories_WhenDuplicateInList_ReturnsFail()
        {
            var reqs = new[]
            {
                new CategoryRequest { Name = "New1" },
                new CategoryRequest { Name = "Existing" }, // duplicate
            };

            _catRepo.Setup(x => x.GetByNameAsync("New1")).ReturnsAsync((Category)null);
            _catRepo.Setup(x => x.GetByNameAsync("Existing")).ReturnsAsync(_existing);

            var result = await _svc.AddMultiCategoriesExecute(reqs);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Category already exists", result.Message);
            _uow.Verify(x => x.RollbackAsync(), Times.Once);
        }

        [Test]
        public async Task AddMultiCategories_WhenAllNew_AddsAndCommits()
        {
            var reqs = new[]
            {
                new CategoryRequest { Name = "C1" },
                new CategoryRequest { Name = "C2" },
            };

            _catRepo
                .Setup(x => x.GetByNameAsync(It.Is<string>(n => n.StartsWith("C"))))
                .ReturnsAsync((Category)null);

            var result = await _svc.AddMultiCategoriesExecute(reqs);

            Assert.IsTrue(result.Success);
            _catRepo.Verify(
                x => x.AddRangeAsync(It.Is<ICollection<Category>>(c => c.Count == 2)),
                Times.Once
            );
            _uow.Verify(x => x.CommitAsync(), Times.Once);
            var responses = (List<CategoryResponse>)result.Data!;
            Assert.AreEqual(2, responses.Count);
        }
    }
}
