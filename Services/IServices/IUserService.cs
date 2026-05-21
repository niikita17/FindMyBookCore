using MyBook_Backend.Models.DTO;

namespace MyBook_Backend.Services.IServices
{
    public interface IUserService
    {

        Task<Result<UserResponseDto>> AddUser(RegisterUserDto model);
        Task<Result<UserResponseDto>> AddUserByAdmin(RegisterUserDto model);

        Task<Result<UserResponseDto>> EditUser(int id, UserResponseDto model);
        Task<Result<UserResponseDto>> DeleteUser(int Id);

        Task <Result<UserResponseDto>> GetUserByEmail(string Email);

        Task<Result<UserResponseDto>> GetUserById(int Id);

        Task<Result<List<UserResponseDto>>> GetAll();
    }
}
