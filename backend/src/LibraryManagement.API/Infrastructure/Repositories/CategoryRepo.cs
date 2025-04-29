using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class CategoryRepo : BaseRepo<Category>
    {
        public CategoryRepo(ApplicationDBContext context)
            : base(context) { }
    }
}
