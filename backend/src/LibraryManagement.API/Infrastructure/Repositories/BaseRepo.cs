using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using LibraryManagement.Core.Application.Interface.Gateways;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Infrastructure.Repositories
{
    public class BaseRepo<TEntity> : IBaseRepo<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDBContext _context;

        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepo(ApplicationDBContext context)
        {
            _context = context;

            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity obj)
        {
            var result = await _dbSet.AddAsync(obj);
            await _context.SaveChangesAsync();

            //Checking what is result return ?
            Console.WriteLine(result);

            return result.Entity;
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity?> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
