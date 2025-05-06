using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.API.Controllers;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Application.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Tests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private CategoryController _controller;

        [SetUp]
        public void Setup()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_categoryServiceMock.Object);
        }

        [Test]
        public async Task CreateCategory_Success_ReturnsOk()
        {
            var request = new CategoryRequest { Name = "Fiction" };
            var result = OperationResult.Ok(new { Id = Guid.NewGuid(), request.Name }, "Created");

            _categoryServiceMock.Setup(s => s.CreateCategoryExecute(request)).ReturnsAsync(result);

            var response = await _controller.CreateCategory(request) as OkObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task CreateCategory_Failure_ReturnsBadRequest()
        {
            var request = new CategoryRequest { Name = "Invalid" };
            var result = OperationResult.Fail("Validation failed");

            _categoryServiceMock.Setup(s => s.CreateCategoryExecute(request)).ReturnsAsync(result);

            var response = await _controller.CreateCategory(request) as BadRequestObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(400, response.StatusCode);
        }

        [Test]
        public async Task GetCategoryById_Success_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var result = OperationResult.Ok(new { Id = id, Name = "Science" }, "Found");

            _categoryServiceMock.Setup(s => s.GetCategoryByIdExecute(id)).ReturnsAsync(result);

            var response = await _controller.GetCategoryById(id) as OkObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task GetCategoryById_NotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var result = OperationResult.Fail("Not found");

            _categoryServiceMock.Setup(s => s.GetCategoryByIdExecute(id)).ReturnsAsync(result);

            var response = await _controller.GetCategoryById(id) as NotFoundObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(404, response.StatusCode);
        }

        [Test]
        public async Task GetAllCategories_Success_ReturnsOk()
        {
            var result = OperationResult.Ok(new List<object>(), "Fetched");

            _categoryServiceMock.Setup(s => s.GetAllCategoriesExecute()).ReturnsAsync(result);

            var response = await _controller.GetAllCategories() as OkObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task GetAllCategories_Failure_ReturnsBadRequest()
        {
            var result = OperationResult.Fail("Failed to fetch");

            _categoryServiceMock.Setup(s => s.GetAllCategoriesExecute()).ReturnsAsync(result);

            var response = await _controller.GetAllCategories() as BadRequestObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(400, response.StatusCode);
        }

        [Test]
        public async Task GetCategoriesPaged_Success_ReturnsOk()
        {
            var result = OperationResult.Ok(new List<object>(), "Paged");

            _categoryServiceMock
                .Setup(s => s.GetCategoriesPagedExecute(1, 10))
                .ReturnsAsync(result);

            var response = await _controller.GetCategoriesPaged(1, 10) as OkObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task GetCategoriesPaged_Failure_ReturnsBadRequest()
        {
            var result = OperationResult.Fail("Invalid paging");

            _categoryServiceMock.Setup(s => s.GetCategoriesPagedExecute(0, 0)).ReturnsAsync(result);

            var response = await _controller.GetCategoriesPaged(0, 0) as BadRequestObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(400, response.StatusCode);
        }

        [Test]
        public async Task UpdateCategory_Success_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var req = new CategoryRequest { Name = "Updated" };
            var result = OperationResult.Ok(new { Id = id, req.Name }, "Updated");

            _categoryServiceMock.Setup(s => s.UpdateCategoryExecute(id, req)).ReturnsAsync(result);

            var response = await _controller.UpdateCategory(id, req) as OkObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task UpdateCategory_Failure_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            var req = new CategoryRequest { Name = "Invalid" };
            var result = OperationResult.Fail("Update failed");

            _categoryServiceMock.Setup(s => s.UpdateCategoryExecute(id, req)).ReturnsAsync(result);

            var response = await _controller.UpdateCategory(id, req) as BadRequestObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(400, response.StatusCode);
        }

        [Test]
        public async Task DeleteCategory_Success_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var result = OperationResult.Ok(null, "Deleted");

            _categoryServiceMock.Setup(s => s.DeleteCategoryExecute(id)).ReturnsAsync(result);

            var response = await _controller.DeleteCategory(id) as OkObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task DeleteCategory_Failure_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var result = OperationResult.Fail("Not found");

            _categoryServiceMock.Setup(s => s.DeleteCategoryExecute(id)).ReturnsAsync(result);

            var response = await _controller.DeleteCategory(id) as NotFoundObjectResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(404, response.StatusCode);
        }
    }
}
