using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyBookBackend.Common.DTO;

using MyBookBackend.Service.IServices;

namespace MyBookBackend.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AdminController> _logger;
        public AdminController(IAdminService adminService, IMemoryCache cache, ILogger<AdminController> logger) {
            _adminService = adminService;
            _cache = cache;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard")]
        public async Task<IActionResult>GetDashboard()
        {
            const string cacheKey =
      "admin_dashboard";

            if (_cache.TryGetValue(
                cacheKey,
                out AdminDashBoardDto dashboard))
            {
                _logger.LogInformation(
                    "Dashboard loaded from CACHE");

                return Ok(dashboard);
            }
            _logger.LogInformation(
     "Dashboard loaded from DATABASE");

            var result =
                await _adminService
                    .GetDashboard();
            _cache.Set(
      cacheKey,
      result,
      TimeSpan.FromMinutes(5));


            return Ok(result);
        }

    }
}
