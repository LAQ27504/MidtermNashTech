using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class UserRepo : BaseRepo<User>, IUserRepo<User>
    {
        public UserRepo(ApplicationDBContext context)
            : base(context) { }
    }
}
