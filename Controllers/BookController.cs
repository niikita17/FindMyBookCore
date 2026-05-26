using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;

using MyBook_Backend.Services.IServices;
using System.Globalization;

namespace MyBook_Backend.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookService
                    .Create(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("getall")]

        public async Task<IActionResult> GetAll(
        string? search,
        int? categoryId,
        int page = 1,
        int pageSize = 10,
        string? sortBy = null)
        {
            var result = await _bookService.GetAll(
                search,
                categoryId,
                page,
                pageSize,
                sortBy
            );

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)

        {
            if (id <= 0)
            {
                return BadRequest("Invalid Id");
            }
            var result =
                await _bookService.Get(id);

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [Authorize]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromForm] CreateProductDto dto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var result = await _bookService.EditbyId(id, dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.Delete(id);
            if (!book.IsSuccess)
                return BadRequest(book.Message);
            return Ok(book);
        }


    }
}
