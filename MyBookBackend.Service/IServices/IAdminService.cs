using MyBookBackend.Common.DTO;

namespace MyBookBackend.Service.IServices
{
    public interface IAdminService
    {
        Task<Result<AdminDashBoardDto>> GetDashboard();
    }
}
