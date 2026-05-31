using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;

namespace MyBook_Backend.Services
{
    public class AdminService
      : IAdminService
    {
        private readonly IAdminRepository  _adminRepository;
        private readonly IMemoryCache _cache;

        public AdminService( IAdminRepository adminRepository, IMemoryCache cache)
        {
            _adminRepository =adminRepository;
            _cache = cache;
        }

        public async Task<  Result<AdminDashBoardDto>>  GetDashboard()
        {


            var totalBooks =
                await _adminRepository
                    .GetTotalBooks();

            var totalUsers =
                await _adminRepository
                    .GetTotalUsers();

            var totalCategories =
                await _adminRepository
                    .GetTotalCategories();

            var books =
                await _adminRepository
                    .GetRecentBooks();

            var users =
                await _adminRepository
                    .GetRecentUsers();

            var recentBooks =
                books.Select(book =>
                    new BookResponseDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Price = book.Price,
                        ImageUrl =
                            book.ImageUrl
                    }).ToList();

            var recentUsers =
                users.Select(user =>
                    new UserResponseDto
                    {
                        Name = user.Name,
                        Email = user.Email,
                        MobileNo =
                            user.MobileNo
                    }).ToList();

            var dashboard =
                new AdminDashBoardDto
                {
                    TotalBooks =
                        totalBooks,

                    TotalUsers =
                        totalUsers,

                    TotalCategories =
                        totalCategories,

                    RecentBooks =
                        recentBooks,

                    RecentUsers =
                        recentUsers
                };

            return Result<AdminDashBoardDto>
                .Success(dashboard);
        }
    }
}
