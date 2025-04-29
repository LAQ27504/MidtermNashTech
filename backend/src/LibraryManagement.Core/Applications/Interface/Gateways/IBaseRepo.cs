namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBaseRepo<TEntity>
        where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity?> GetByIdAsync(Guid id);

        Task<ICollection<TEntity>> GetAllAsync();

        Task<TEntity?> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);
    }
}
