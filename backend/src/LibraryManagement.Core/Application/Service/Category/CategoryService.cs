using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.DTOs.Responses;
using LibraryManagement.Core.Application.Interface.Gateways;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Domains.Entities;

namespace LibraryManagement.Core.Domains.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepo _categoryRepo;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepo categoryRepo)
        {
            _unitOfWork = unitOfWork;
            _categoryRepo = categoryRepo;
        }

        public async Task<OperationResult> CreateCategoryExecute(CategoryRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingCategory = await _categoryRepo.GetByNameAsync(request.Name);
                if (existingCategory != null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("Category already exists");
                }

                var category = new Category { Id = Guid.NewGuid(), Name = request.Name };

                await _categoryRepo.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return OperationResult.Ok(
                    new CategoryResponse { Id = category.Id, Name = category.Name },
                    "Category created successfully."
                );
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> GetCategoryByIdExecute(Guid id)
        {
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category == null)
                {
                    return OperationResult.Fail("Category not found");
                }

                return OperationResult.Ok(
                    new CategoryResponse { Id = category.Id, Name = category.Name },
                    "Category retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> GetAllCategoriesExecute()
        {
            try
            {
                var categories = await _categoryRepo.GetAllAsync();
                var response = categories
                    .Select(c => new CategoryResponse { Id = c.Id, Name = c.Name })
                    .ToList();
                return OperationResult.Ok(response, "Categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> UpdateCategoryExecute(Guid id, CategoryRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("Category not found");
                }

                if (category.Name != request.Name)
                {
                    var existing = await _categoryRepo.GetByNameAsync(request.Name);
                    if (existing != null && existing.Id != id)
                    {
                        await _unitOfWork.RollbackAsync();
                        return OperationResult.Fail("Category with this name already exists");
                    }
                }

                category.Name = request.Name;
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return OperationResult.Ok(
                    new CategoryResponse { Id = category.Id, Name = category.Name },
                    "Category updated successfully."
                );
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> DeleteCategoryExecute(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category == null)
                {
                    await _unitOfWork.RollbackAsync();
                    return OperationResult.Fail("Category not found");
                }

                _categoryRepo.Delete(category);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return OperationResult.Ok("Category deleted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> GetCategoriesPagedExecute(int pageNumber, int pageSize)
        {
            try
            {
                var (categories, total) = await _categoryRepo.GetPagedAsync(pageNumber, pageSize);
                var responses = categories
                    .Select(c => new CategoryResponse { Id = c.Id, Name = c.Name })
                    .ToList();

                var pagedResponse = new PaginationResponse<CategoryResponse>
                {
                    Items = responses,
                    TotalCount = total,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };

                return OperationResult.Ok(pagedResponse, "Categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail(ex.Message);
            }
        }

        public async Task<OperationResult> AddMultiCategoriesExecute(
            ICollection<CategoryRequest> requests
        )
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var categories = new List<Category>();
                foreach (var request in requests)
                {
                    var existingCategory = await _categoryRepo.GetByNameAsync(request.Name);
                    if (existingCategory != null)
                    {
                        await _unitOfWork.RollbackAsync();
                        return OperationResult.Fail("Category already exists");
                    }

                    var category = new Category { Id = Guid.NewGuid(), Name = request.Name };
                    categories.Add(category);
                }

                await _categoryRepo.AddRangeAsync(categories);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                var responses = categories
                    .Select(c => new CategoryResponse { Id = c.Id, Name = c.Name })
                    .ToList();

                return OperationResult.Ok(responses, "Categories created successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return OperationResult.Fail(ex.Message);
            }
        }
    }
}
