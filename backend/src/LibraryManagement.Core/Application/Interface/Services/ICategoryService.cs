using LibraryManagement.Core.Application.DTOs.Requests;

namespace LibraryManagement.Core.Application.Interface.Services
{
    public interface ICategoryService
    {
        Task<OperationResult> CreateCategoryExecute(CategoryRequest request);
        Task<OperationResult> GetCategoryByIdExecute(Guid id);
        Task<OperationResult> GetAllCategoriesExecute();
        Task<OperationResult> GetCategoriesPagedExecute(int pageNumber, int pageSize);
        Task<OperationResult> UpdateCategoryExecute(Guid id, CategoryRequest request);
        Task<OperationResult> DeleteCategoryExecute(Guid id);
        Task<OperationResult> AddMultiCategoriesExecute(ICollection<CategoryRequest> requests);
    }
}
