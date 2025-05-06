using System;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest category)
        {
            var res = await _categoryService.CreateCategoryExecute(category);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpPost("multi")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> CreateMultipleCategories(
            [FromBody] List<CategoryRequest> categories
        )
        {
            var res = await _categoryService.AddMultiCategoriesExecute(categories);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var res = await _categoryService.GetCategoryByIdExecute(id);
            if (!res.Success)
            {
                return NotFound(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var res = await _categoryService.GetAllCategoriesExecute();
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        [Authorize]
        public async Task<IActionResult> GetCategoriesPaged(int pageNumber, int pageSize)
        {
            var res = await _categoryService.GetCategoriesPagedExecute(pageNumber, pageSize);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> UpdateCategory(
            Guid id,
            [FromBody] CategoryRequest category
        )
        {
            var res = await _categoryService.UpdateCategoryExecute(id, category);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var res = await _categoryService.DeleteCategoryExecute(id);
            if (!res.Success)
            {
                return NotFound(res.Message);
            }
            return Ok(new { res.Message });
        }
    }
}
