using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;

namespace MyBook_Backend.Services
{
    public class AuthService : IAuthService
    {
        public readonly IAuthRepository _authRepository;
        public readonly IUserRepository _userRepository;
        public AuthService(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository= userRepository;

        }
        public async Task<Result<LoggedInUserDto>> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Result<LoggedInUserDto>.Failure("Invalid request");

            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return Result<LoggedInUserDto>.Failure("User not found");

           

            if (user.Password != password)
                return Result<LoggedInUserDto>.Failure("Username or password is incorrect");

           LoggedInUserDto data=await _authRepository.Login(email,password);
     
            return Result<LoggedInUserDto>.Success(data, "User logged in successfully");


        }

    }
}
