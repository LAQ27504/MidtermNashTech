using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Application.Service;
using LibraryManagement.Core.Application.Service.Security;
using LibraryManagement.Core.Domains.Entities;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test.Services
{
    [TestFixture]
    public class BookBorrowingRequestServiceTests
    {
        private Mock<IBookBorrowingRequestRepo> _reqRepo;
        private Mock<IBookBorrowingRequestDetailsRepo> _detailsRepo;
        private Mock<IBookService> _bookService;
        private Mock<IUnitOfWork> _uow;
        private Mock<TokenInfoService> _tokenSvc;
        private BookBorrowingRequestService _svc;

        private readonly Guid _userId = Guid.NewGuid();
        private const string _userName = "user1";
        private const string _userRole = nameof(UserType.NormalUser);

        [SetUp]
        public void SetUp()
        {
            _reqRepo = new Mock<IBookBorrowingRequestRepo>();
            _detailsRepo = new Mock<IBookBorrowingRequestDetailsRepo>();
            _bookService = new Mock<IBookService>();
            _uow = new Mock<IUnitOfWork>();
            _tokenSvc = new Mock<TokenInfoService>();

            _tokenSvc.Setup(x => x.GetTokenInfo()).Returns((_userId, _userName, _userRole));

            _uow.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.CommitAsync()).Returns(Task.CompletedTask);
            _uow.Setup(x => x.RollbackAsync()).Returns(Task.CompletedTask);

            _svc = new BookBorrowingRequestService(
                _reqRepo.Object,
                _detailsRepo.Object,
                _bookService.Object,
                _uow.Object,
                _tokenSvc.Object
            );
        }

        [Test]
        public async Task CreateBorrowRequest_LimitReached_Fails()
        {
            _reqRepo.Setup(x => x.GetNumOfRequestThisMonth(_userId)).ReturnsAsync(5);

            var req = new BorrowRequest
            {
                RequestorID = _userId,
                BookIds = new List<Guid> { Guid.NewGuid() },
            };

            var op = await _svc.CreateBorrowRequestAsync(req);

            Assert.IsFalse(op.Success);
            StringAssert.Contains("Request limit reached", op.Message);
        }

        [Test]
        public async Task CreateBorrowRequest_Valid_ReturnsResponse()
        {
            _reqRepo.Setup(x => x.GetNumOfRequestThisMonth(_userId)).ReturnsAsync(0);

            _bookService
                .Setup(x => x.DecreaseAvailableAmount(It.IsAny<Guid>(), 1))
                .ReturnsAsync(true);

            Guid headerId = Guid.Empty;
            _reqRepo
                .Setup(x => x.AddAsync(It.IsAny<BookBorrowingRequest>()))
                .Callback<BookBorrowingRequest>(h => headerId = h.Id)
                .Returns(Task.CompletedTask);

            var expected = new BorrowResponse { Id = Guid.NewGuid() };
            _reqRepo
                .Setup(x => x.GetBorrowResponsesByRequestIdAsync(It.IsAny<Guid>()))
                .Returns(
                    Task.FromResult<ICollection<BorrowResponse>>(
                        new List<BorrowResponse> { expected }
                    )
                );

            var req = new BorrowRequest
            {
                RequestorID = _userId,
                BookIds = new List<Guid> { Guid.NewGuid() },
            };

            var op = await _svc.CreateBorrowRequestAsync(req);

            Assert.IsTrue(op.Success);
            Assert.IsInstanceOf<BorrowResponse>(op.Data);
            Assert.AreEqual(expected.Id, ((BorrowResponse)op.Data).Id);
            _uow.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllRequests_ReturnsList()
        {
            var sample = new BorrowResponse { Id = Guid.NewGuid() };
            _reqRepo
                .Setup(x => x.GetAllRequestsWithDetailsAsync())
                .Returns(Task.FromResult<ICollection<BorrowResponse>>(new[] { sample }));

            var resp = new BorrowResponse { Id = sample.Id };
            _reqRepo
                .Setup(x => x.GetBorrowResponsesByRequestIdAsync(sample.Id))
                .Returns(
                    Task.FromResult<ICollection<BorrowResponse>>(new List<BorrowResponse> { resp })
                );

            var op = await _svc.GetAllRequestsAsync();
            Assert.IsTrue(op.Success);

            var list = (List<BorrowResponse>)op.Data;
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(sample.Id, list[0].Id);
        }

        [Test]
        public async Task GetById_Missing_ReturnsFail()
        {
            _reqRepo
                .Setup(x => x.GetBorrowResponsesByRequestIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<ICollection<BorrowResponse>>(new List<BorrowResponse>()));

            var op = await _svc.GetBorrowRequestByIdAsync(Guid.NewGuid());
            Assert.IsFalse(op.Success);
            Assert.AreEqual("Request not found.", op.Message);
        }

        [Test]
        public async Task GetById_Exists_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var br = new BorrowResponse { Id = id };

            _reqRepo
                .Setup(x => x.GetBorrowResponsesByRequestIdAsync(id))
                .Returns(
                    Task.FromResult<ICollection<BorrowResponse>>(new List<BorrowResponse> { br })
                );

            var op = await _svc.GetBorrowRequestByIdAsync(id);
            Assert.IsTrue(op.Success);
            Assert.AreEqual(id, ((BorrowResponse)op.Data).Id);
        }

        [Test]
        public async Task GetByUser_ReturnsList()
        {
            var sample = new BookBorrowingRequest { Id = Guid.NewGuid(), RequestorId = _userId };
            _reqRepo
                .Setup(x => x.GetRequestsWithDetailsByUserAsync(_userId))
                .Returns(
                    Task.FromResult<List<BookBorrowingRequest>>(
                        new List<BookBorrowingRequest> { sample }
                    )
                );

            var resp = new BorrowResponse { Id = sample.Id };
            _reqRepo
                .Setup(x => x.GetBorrowResponsesByRequestIdAsync(sample.Id))
                .Returns(
                    Task.FromResult<ICollection<BorrowResponse>>(new List<BorrowResponse> { resp })
                );

            var op = await _svc.GetRequestsByUserAsync(_userId);
            Assert.IsTrue(op.Success);
            Assert.AreEqual(1, ((List<BorrowResponse>)op.Data).Count);
        }

        [Test]
        public async Task ReturnBooks_Valid_ProcessesReturns()
        {
            var detail = new BookBorrowingRequestDetails
            {
                Id = Guid.NewGuid(),
                BookId = Guid.NewGuid(),
                Status = BorrowBookStatus.Approved,
            };
            var header = new BookBorrowingRequest
            {
                Id = Guid.NewGuid(),
                RequestorId = _userId,
                BookBorrowingRequestDetails = new List<BookBorrowingRequestDetails> { detail },
            };

            _reqRepo.Setup(x => x.GetByIdAsync(header.Id)).ReturnsAsync(header);

            _bookService
                .Setup(x => x.IncreaseAvailableAmount(detail.BookId, 1))
                .Returns(Task.CompletedTask);

            _detailsRepo
                .Setup(x => x.UpdateRequestDetailStatusAsync(header.Id, BorrowBookStatus.Approved))
                .Returns(Task.CompletedTask);

            var br = new BorrowResponse { Id = header.Id };
            _reqRepo
                .Setup(x => x.GetBorrowResponsesByRequestIdAsync(header.Id))
                .Returns(
                    Task.FromResult<ICollection<BorrowResponse>>(new List<BorrowResponse> { br })
                );

            var op = await _svc.ReturnBooksAsync(
                new ReturnRequest
                {
                    RequestId = header.Id,
                    BookIds = new List<Guid> { detail.BookId },
                }
            );

            Assert.IsTrue(op.Success);
            Assert.AreEqual("Books returned successfully.", op.Message);
            _detailsRepo.Verify(
                x =>
                    x.Update(
                        It.Is<BookBorrowingRequestDetails>(d =>
                            d.Status == BorrowBookStatus.Returned
                        )
                    ),
                Times.Once
            );
            _uow.Verify(x => x.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task SetApproval_Valid_ApprovesAndCommits()
        {
            var header = new BookBorrowingRequest
            {
                Id = Guid.NewGuid(),
                Status = RequestStatus.Waiting,
            };
            _reqRepo.Setup(x => x.GetByIdAsync(header.Id)).ReturnsAsync(header);

            _tokenSvc
                .Setup(x => x.GetTokenInfo())
                .Returns((_userId, _userName, nameof(UserType.SuperUser)));

            _detailsRepo
                .Setup(x => x.UpdateRequestDetailStatusAsync(header.Id, BorrowBookStatus.Approved))
                .Returns(Task.CompletedTask);

            _reqRepo.Setup(x => x.Update(header));

            var br = new BorrowResponse { Id = header.Id };
            _reqRepo
                .Setup(x => x.GetBorrowResponsesByRequestIdAsync(header.Id))
                .Returns(
                    Task.FromResult<ICollection<BorrowResponse>>(new List<BorrowResponse> { br })
                );

            var op = await _svc.SetApprovalAsync(header.Id, true);

            Assert.IsTrue(op.Success);
            Assert.AreEqual("Request approved.", op.Message);
            _reqRepo.Verify(
                x =>
                    x.Update(
                        It.Is<BookBorrowingRequest>(r =>
                            r.Status == RequestStatus.Approved && r.ApproverId == _userId
                        )
                    ),
                Times.Once
            );
            _uow.Verify(x => x.CommitAsync(), Times.Once);
        }
    }
}
