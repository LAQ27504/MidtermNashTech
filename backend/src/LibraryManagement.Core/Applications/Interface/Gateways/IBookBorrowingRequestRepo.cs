namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBookBorrowingRequestRepo<TEntity> : IBaseRepo<TEntity>
        where TEntity : class { }
}
