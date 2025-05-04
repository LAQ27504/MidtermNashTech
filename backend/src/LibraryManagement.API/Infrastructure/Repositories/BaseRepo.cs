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

        public async Task AddAsync(TEntity obj)
        {
            var result = await _dbSet.AddAsync(obj);
            //Checking what is result return ?
            Console.WriteLine(result);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var obj = await _dbSet.FindAsync(id);

            if (obj == null)
            {
                return false;
            }

            _dbSet.Remove(obj);

            return true;
        }

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            var obj = await _dbSet.FindAsync(id);

            return obj;
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<(ICollection<TEntity>, int)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _dbSet.CountAsync();
            var obj = await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (obj, totalRecords);
        }

        public async Task AddRangeAsync(ICollection<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
    }
}
