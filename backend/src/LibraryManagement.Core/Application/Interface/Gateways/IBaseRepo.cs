namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBaseRepo<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);

        Task<TEntity?> GetByIdAsync(Guid id);

        Task<ICollection<TEntity>> GetAllAsync();

        Task AddRangeAsync(ICollection<TEntity> entities);

        Task<(ICollection<TEntity>, int)> GetPagedAsync(int pageNumber, int pageSize);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<bool> DeleteById(Guid id);
    }
}
