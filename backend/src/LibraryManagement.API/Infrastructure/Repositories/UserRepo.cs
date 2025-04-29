using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class UserRepo : BaseRepo<User>
    {
        public UserRepo(ApplicationDBContext context)
            : base(context) { }
    }
}
