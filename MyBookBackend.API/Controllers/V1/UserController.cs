using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBookBackend.Common.DTO;
using MyBookBackend.Service.IServices;
using System.ComponentModel.DataAnnotations;


namespace MyBookBackend.API.Controllers.V1
{

    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/user")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IValidator<RegisterUserDto> _validator;
        public UserController(
        IUserService userService,
        IValidator<RegisterUserDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post(RegisterUserDto user)
        {
            var validationResult =
                await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result =
                await _userService.AddUser(user);

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
