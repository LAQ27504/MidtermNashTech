using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Interface.Services;
using LibraryManagement.Core.Domains.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Expressions;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> CreateBook([FromBody] BookRequest book)
        {
            var res = await _bookService.CreateBookExecute(book);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpPost("multi")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> AddMultiBooks([FromBody] ICollection<BookRequest> books)
        {
            var res = await _bookService.AddMultiBooksExecute(books);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var res = await _bookService.GetBookByIdExecute(id);
            if (!res.Success)
            {
                return NotFound(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBooks()
        {
            var res = await _bookService.GetAllBooksExecute();
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        [Authorize]
        public async Task<IActionResult> GetBookPagination(int pageNumber, int pageSize)
        {
            var res = await _bookService.GetBookPagedExecute(pageNumber, pageSize);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookRequest book)
        {
            var res = await _bookService.UpdateBookExecute(id, book);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var res = await _bookService.DeleteBookExecute(id);
            if (!res.Success)
            {
                return NotFound(res.Message);
            }
            return Ok(new { res.Message });
        }
    }
}
