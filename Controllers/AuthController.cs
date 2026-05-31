using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;
using Serilog;
using System.Security.Claims;

namespace MyBook_Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {

            _authService = authService;
            _logger = logger;
        }


        //post login
        [HttpPost("login")]

     
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {

      
            if (request == null)
                return BadRequest("Invalid request");

            var result = await _authService.Login
                (request.Email, request.Password);
          
            if (result == null)
            {
                return BadRequest();
            }
            Console.WriteLine(result.Message);
            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }
            var refreshToken = result.Data.RefreshToken;
            Response.Cookies.Append(
           "refreshToken",
           refreshToken,
           new CookieOptions
           {
               HttpOnly = true,
               Secure = false,
               SameSite = SameSiteMode.None,
               Expires =
                   DateTime.UtcNow
                       .AddDays(7)
           });


            return Ok(result.Data.AccessToken);
        }


  
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken =
                Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var result =
                await _authService.RefreshToken(refreshToken);

            if (result == null || !result.IsSuccess)
            {
                return Unauthorized();
            }

            Response.Cookies.Append(
                "refreshToken",
                result.Data.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

            return Ok(result.Data.AccessToken);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            int userId = int.Parse(
      User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            await _authService.Logout(userId);
            Response.Cookies.Delete(
    "refreshToken",
    new CookieOptions
    {
        HttpOnly = true,
        Secure = false,
        SameSite = SameSiteMode.None
    });

            return Ok();
        }
    }
}
