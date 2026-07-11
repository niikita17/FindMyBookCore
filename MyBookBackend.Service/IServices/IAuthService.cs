using MyBookBackend.Common.DTO;

namespace MyBookBackend.Service.IServices
{
    public interface IAuthService
    {
        public Task<Result<LoggedInUserDto>> Login(string email, string password);
        public  Task<Result<LoggedInUserDto>>RefreshToken(string refreshToken);
        public Task Logout(int userId);
        int getLoggedInUserId { get; }
    }
}
