using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;

namespace MyBook_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService) {
            _adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard")]
        public async Task<IActionResult>GetDashboard()
        {
            var result =
                await _adminService
                    .GetDashboard();

            return Ok(result);
        }

    }
}
