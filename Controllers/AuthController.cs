using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;

namespace MyBook_Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
  
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {

            _authRepository = authRepository;
        }


        //post login
        [HttpPost("login")]

     
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (request == null)
                return BadRequest("Invalid request");

            var result = await _authRepository.Login(request.Email, request.Password);

        

            return Ok(result);
        }


    }
}
