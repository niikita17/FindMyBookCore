using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Repository.IRepository;

namespace MyBook_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AdminController(IUserRepository userRepository) { 
        _userRepository = userRepository;
        }



    }
}
