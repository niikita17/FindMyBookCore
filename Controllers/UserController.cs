using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Services.IServices;
using System.ComponentModel.DataAnnotations;


namespace MyBook_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(RegisterUserDto user)
        {
            if (user == null)
                return BadRequest("All feilds are required");

          
            var result = await _userService.AddUser(user);

            return Ok(result);
        }

        //edit user
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put(int id,UserResponseDto user)
        {

            if (user == null || id==null)
                return Unauthorized("feilds cannot be null");

            var result = await _userService.EditUser(id,user);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);

        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
                return BadRequest("id cannot be null");
            var result =await _userService.DeleteUser(id);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if (id == null)
            {
                return BadRequest("Id cannot be null");
            }
            var user= await _userService.GetUserById(id);
            return Ok(user);

        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _userService.GetAll();

            return Ok(res);
        }


    }
}
