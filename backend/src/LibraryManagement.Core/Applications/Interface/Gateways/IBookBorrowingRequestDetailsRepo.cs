namespace LibraryManagement.Core.Application.Interface.Gateways
{
    public interface IBookBorrowingRequestDetailsRepo<TEntity> : IBaseRepo<TEntity>
        where TEntity : class { }
}
