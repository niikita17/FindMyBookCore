using MyBook_Backend.Models.DTO;

namespace MyBook_Backend.Services.IServices
{
    public interface IAdminService
    {
        Task<Result<AdminDashBoardDto>> GetDashboard();
    }
}
