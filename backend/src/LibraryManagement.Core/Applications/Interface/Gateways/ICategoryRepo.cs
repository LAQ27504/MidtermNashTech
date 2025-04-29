namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface ICategoryRepo<TEntity> : IBaseRepo<TEntity>
        where TEntity : class { }
}
