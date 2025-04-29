using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.API.Infrastructure.Repositories;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Infrastructure.Repositories
{
    public class CategoryRepo : BaseRepo<Category>, ICategoryRepo<Category>
    {
        public CategoryRepo(ApplicationDBContext context)
            : base(context) { }
    }
}
