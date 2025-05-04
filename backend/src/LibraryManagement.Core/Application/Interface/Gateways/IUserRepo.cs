using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IUserRepo : IBaseRepo<User>
    {
        public Task<User?> GetUserByName(string name);
    }
}
