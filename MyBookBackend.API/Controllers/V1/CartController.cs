using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MyBookBackend.Common.DTO;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Service.IServices;
using System.Security.Claims;

namespace MyBookBackend.API.Controllers.V1
{
    [ApiController]
  
    [Authorize]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/cart")]
    public class CartController
        : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(
            ICartService cartService
        )
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart( AddToCartDto dto )
        {
            int userId =
                int.Parse( 
                    User.FindFirst(
                        ClaimTypes
                        .NameIdentifier
                    )?.Value
                );

            var result =
                await _cartService
                    .AddToCart(
                        userId,
                        dto
                    );

            if (!result.IsSuccess)
            {


                return BadRequest(
                    result
                );
            }

            return Ok(result);
        }


        [HttpGet("mycart")]
        public async Task<IActionResult> GetMyCart()  {
            int userId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes
                        .NameIdentifier
                    )?.Value
                );

            var result =
                await _cartService.GetCartByUser( userId );
            

            if (!result.IsSuccess)
            {
                return BadRequest(
                    result
                );
            }
         

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (id == null)
                return BadRequest("id is null");

            var result= await _cartService.Remove( id );

            return Ok(result);


        }
    }
}
