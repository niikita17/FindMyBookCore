using MyBook_Backend.Models.DTO;

namespace MyBook_Backend.Services.IServices
{
    public interface IAuthService
    {
        public Task<Result<LoggedInUserDto>> Login(string email, string password);
        public  Task<Result<LoggedInUserDto>>RefreshToken(string refreshToken);
        public Task Logout(int userId);
    }
}
