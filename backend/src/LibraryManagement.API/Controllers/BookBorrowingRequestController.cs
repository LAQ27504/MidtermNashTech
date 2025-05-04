using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookBorrowingRequestController : ControllerBase
    {
        private readonly IBookBorrowingRequestService _service;

        public BookBorrowingRequestController(IBookBorrowingRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBorrowRequest([FromBody] BorrowRequest request)
        {
            var res = await _service.CreateBorrowRequestAsync(request);
            if (!res.Success)
                return BadRequest(res.Message);
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetBorrowRequestById(Guid id)
        {
            var res = await _service.GetBorrowRequestByIdAsync(id);
            if (!res.Success)
                return NotFound(res.Message);
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("user/{requestorId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetRequestsByUser(Guid requestorId)
        {
            var res = await _service.GetRequestsByUserAsync(requestorId);
            if (!res.Success)
                return BadRequest(res.Message);
            return Ok(new { res.Data, res.Message });
        }

        [HttpPost("return")]
        [Authorize]
        public async Task<IActionResult> ReturnBooks([FromBody] ReturnRequest request)
        {
            var res = await _service.ReturnBooksAsync(request);
            if (!res.Success)
                return BadRequest(res.Message);
            return Ok(new { res.Data, res.Message });
        }

        [HttpPost("approve")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> SetApproval([FromBody] ApprovalRequest request)
        {
            var res = await _service.SetApprovalAsync(
                request.RequestId,
                request.ApproverId,
                request.IsApproved
            );
            if (!res.Success)
                return BadRequest(res.Message);
            return Ok(new { res.Data, res.Message });
        }
    }
}
