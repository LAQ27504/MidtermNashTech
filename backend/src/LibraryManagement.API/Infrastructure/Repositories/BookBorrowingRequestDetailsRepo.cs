using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Identity.Client;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class BookBorrowingRequestDetailsRepo
        : BaseRepo<BookBorrowingRequestDetails>,
            IBookBorrowingRequestDetailsRepo<BookBorrowingRequestDetails>
    {
        public BookBorrowingRequestDetailsRepo(ApplicationDBContext context)
            : base(context) { }
    }
}
