using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface ICategoryRepo : IBaseRepo<Category>
    {
        public Task<Category> GetByNameAsync(string name);
    }
}
