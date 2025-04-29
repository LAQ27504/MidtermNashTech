namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBookRepo<TEntity> : IBaseRepo<TEntity>
        where TEntity : class { }
}
