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
    public class BookBorrowingRequestControllerTests
    {
        private Mock<IBookBorrowingRequestService> _serviceMock;
        private BookBorrowingRequestController _controller;

        private BorrowRequest _borrowRequest;
        private ReturnRequest _returnRequest;
        private ApprovalRequest _approvalRequest;
        private Guid _requestId;
        private Guid _requestorId;

        [SetUp]
        public void SetUp()
        {
            _requestId = Guid.NewGuid();
            _requestorId = Guid.NewGuid();

            _borrowRequest = new BorrowRequest { RequestorID = _requestorId };
            _returnRequest = new ReturnRequest { RequestId = _requestorId };
            _approvalRequest = new ApprovalRequest { RequestId = _requestId, IsApproved = true };

            _serviceMock = new Mock<IBookBorrowingRequestService>();
            _controller = new BookBorrowingRequestController(_serviceMock.Object);
        }

        [Test]
        public async Task CreateBorrowRequest_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s => s.CreateBorrowRequestAsync(_borrowRequest))
                .ReturnsAsync(OperationResult.Ok("Created"));

            var result = await _controller.CreateBorrowRequest(_borrowRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task CreateBorrowRequest_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(s => s.CreateBorrowRequestAsync(_borrowRequest))
                .ReturnsAsync(OperationResult.Fail("Error"));

            var result =
                await _controller.CreateBorrowRequest(_borrowRequest) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetAllBorrowRequests_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s => s.GetAllRequestsAsync())
                .ReturnsAsync(OperationResult.Ok(new List<BorrowResponse>()));

            var result = await _controller.GetAllBorrowRequests() as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetAllBorrowRequests_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(s => s.GetAllRequestsAsync())
                .ReturnsAsync(OperationResult.Fail("Failed"));

            var result = await _controller.GetAllBorrowRequests() as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetBorrowRequestById_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s => s.GetBorrowRequestByIdAsync(_requestId))
                .ReturnsAsync(OperationResult.Ok(new BorrowResponse()));

            var result = await _controller.GetBorrowRequestById(_requestId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetBorrowRequestById_NotFound_ReturnsNotFound()
        {
            _serviceMock
                .Setup(s => s.GetBorrowRequestByIdAsync(_requestId))
                .ReturnsAsync(OperationResult.Fail("Not found"));

            var result = await _controller.GetBorrowRequestById(_requestId) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [Test]
        public async Task GetRequestsByUser_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s => s.GetRequestsByUserAsync(_requestorId))
                .ReturnsAsync(OperationResult.Ok(new List<BorrowResponse>()));

            var result = await _controller.GetRequestsByUser(_requestorId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetRequestsByUser_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(s => s.GetRequestsByUserAsync(_requestorId))
                .ReturnsAsync(OperationResult.Fail("Fail"));

            var result =
                await _controller.GetRequestsByUser(_requestorId) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task ReturnBooks_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s => s.ReturnBooksAsync(_returnRequest))
                .ReturnsAsync(OperationResult.Ok("Returned"));

            var result = await _controller.ReturnBooks(_returnRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task ReturnBooks_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(s => s.ReturnBooksAsync(_returnRequest))
                .ReturnsAsync(OperationResult.Fail("Return failed"));

            var result = await _controller.ReturnBooks(_returnRequest) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task SetApproval_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s =>
                    s.SetApprovalAsync(_approvalRequest.RequestId, _approvalRequest.IsApproved)
                )
                .ReturnsAsync(OperationResult.Ok("Approved"));

            var result = await _controller.SetApproval(_approvalRequest) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task SetApproval_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(s =>
                    s.SetApprovalAsync(_approvalRequest.RequestId, _approvalRequest.IsApproved)
                )
                .ReturnsAsync(OperationResult.Fail("Failed"));

            var result = await _controller.SetApproval(_approvalRequest) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetBookPaginationUserId_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s => s.GetRequestWithPaginationUserIdAsync(1, 10, _requestorId))
                .ReturnsAsync(OperationResult.Ok("Paged"));

            var result =
                await _controller.GetBookPaginationUserId(1, 10, _requestorId) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetBookPaginationUserId_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(s => s.GetRequestWithPaginationUserIdAsync(1, 10, _requestorId))
                .ReturnsAsync(OperationResult.Fail("Invalid"));

            var result =
                await _controller.GetBookPaginationUserId(1, 10, _requestorId)
                as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public async Task GetRequestWithPaginationWaiting_Success_ReturnsOk()
        {
            _serviceMock
                .Setup(s => s.GetRequestWithPaginationWaitingAsync(1, 10))
                .ReturnsAsync(OperationResult.Ok("Waiting List"));

            var result = await _controller.GetRequestWithPaginationWaiting(1, 10) as OkObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task GetRequestWithPaginationWaiting_Failure_ReturnsBadRequest()
        {
            _serviceMock
                .Setup(s => s.GetRequestWithPaginationWaitingAsync(1, 10))
                .ReturnsAsync(OperationResult.Fail("Error"));

            var result =
                await _controller.GetRequestWithPaginationWaiting(1, 10) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
