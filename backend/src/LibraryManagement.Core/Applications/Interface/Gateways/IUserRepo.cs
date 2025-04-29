namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IUserRepo<TEntity> : IBaseRepo<TEntity>
        where TEntity : class { }
}
