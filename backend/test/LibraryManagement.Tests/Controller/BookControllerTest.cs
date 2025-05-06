using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.API.Controllers;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test.Controllers
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookService> _serviceMock;
        private BookController _controller;
        private Guid _bookId;
        private BookRequest _bookRequest;

        [SetUp]
        public void SetUp()
        {
            _bookId = Guid.NewGuid();
            _bookRequest = new BookRequest
            {
                Name = "Sample Book",
                Author = "Author",
                Amount = 5,
                CategoryId = Guid.NewGuid(),
            };

            _serviceMock = new Mock<IBookService>();
            _controller = new BookController(_serviceMock.Object);
        }

        [Test]
        public async Task CreateBook_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(x => x.CreateBookExecute(It.IsAny<BookRequest>()))
                .ReturnsAsync(OperationResult.Ok("Created"));

            var result = await _controller.CreateBook(_bookRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task CreateBook_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(x => x.CreateBookExecute(It.IsAny<BookRequest>()))
                .ReturnsAsync(OperationResult.Fail("Error"));

            var result = await _controller.CreateBook(_bookRequest) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetBookById_Found_ReturnsOk()
        {
            var bookRes = new BookResponse { Id = _bookId };
            _serviceMock
                .Setup(x => x.GetBookByIdExecute(_bookId))
                .ReturnsAsync(OperationResult.Ok(bookRes));

            var result = await _controller.GetBookById(_bookId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetBookById_NotFound_ReturnsNotFound()
        {
            _serviceMock
                .Setup(x => x.GetBookByIdExecute(_bookId))
                .ReturnsAsync(OperationResult.Fail("Not found"));

            var result = await _controller.GetBookById(_bookId) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task GetAllBooks_ReturnsOk()
        {
            _serviceMock
                .Setup(x => x.GetAllBooksExecute())
                .ReturnsAsync(OperationResult.Ok(new List<BookResponse>()));

            var result = await _controller.GetAllBooks() as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetAllBooks_Fails_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(x => x.GetAllBooksExecute())
                .ReturnsAsync(OperationResult.Fail("Fail"));

            var result = await _controller.GetAllBooks() as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetBookPagination_Valid_ReturnsOk()
        {
            _serviceMock
                .Setup(x => x.GetBookPagedExecute(1, 10))
                .ReturnsAsync(OperationResult.Ok(new PaginationResponse<BookResponse>()));

            var result = await _controller.GetBookPagination(1, 10) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetBookPagination_Invalid_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(x => x.GetBookPagedExecute(0, 0))
                .ReturnsAsync(OperationResult.Fail("Invalid"));

            var result = await _controller.GetBookPagination(0, 0) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task UpdateBook_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(x => x.UpdateBookExecute(_bookId, _bookRequest))
                .ReturnsAsync(OperationResult.Ok("Updated"));

            var result = await _controller.UpdateBook(_bookId, _bookRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateBook_Fail_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(x => x.UpdateBookExecute(_bookId, _bookRequest))
                .ReturnsAsync(OperationResult.Fail("Update failed"));

            var result =
                await _controller.UpdateBook(_bookId, _bookRequest) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task DeleteBook_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(x => x.DeleteBookExecute(_bookId))
                .ReturnsAsync(OperationResult.Ok("Deleted"));

            var result = await _controller.DeleteBook(_bookId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task DeleteBook_Fail_ReturnsNotFound()
        {
            _serviceMock
                .Setup(x => x.DeleteBookExecute(_bookId))
                .ReturnsAsync(OperationResult.Fail("Not found"));

            var result = await _controller.DeleteBook(_bookId) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task AddMultiBooks_Success_ReturnsOk()
        {
            var requests = new List<BookRequest> { _bookRequest };

            _serviceMock
                .Setup(x => x.AddMultiBooksExecute(requests))
                .ReturnsAsync(OperationResult.Ok("Added"));

            var result = await _controller.AddMultiBooks(requests) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task AddMultiBooks_Fail_ReturnsBadRequest()
        {
            var requests = new List<BookRequest> { _bookRequest };

            _serviceMock
                .Setup(x => x.AddMultiBooksExecute(requests))
                .ReturnsAsync(OperationResult.Fail("One or more categories not found."));

            var result = await _controller.AddMultiBooks(requests) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
