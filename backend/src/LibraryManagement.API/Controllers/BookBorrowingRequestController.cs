using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBorrowRequests()
        {
            var res = await _service.GetAllRequestsAsync();
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
            var res = await _service.SetApprovalAsync(request.RequestId, request.IsApproved);
            if (!res.Success)
                return BadRequest(res.Message);
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("{pageNumber:int}/{pageSize:int}/{requestorId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetBookPaginationUserId(
            int pageNumber,
            int pageSize,
            Guid requestorId
        )
        {
            var res = await _service.GetRequestWithPaginationUserIdAsync(
                pageNumber,
                pageSize,
                requestorId
            );
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [HttpGet("waiting/{pageNumber:int}/{pageSize:int}")]
        [Authorize(Roles = nameof(UserType.SuperUser))]
        public async Task<IActionResult> GetRequestWithPaginationWaiting(
            int pageNumber,
            int pageSize
        )
        {
            var res = await _service.GetRequestWithPaginationWaitingAsync(pageNumber, pageSize);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }
    }
}
