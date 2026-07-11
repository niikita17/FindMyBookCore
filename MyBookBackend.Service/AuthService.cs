using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;
using MyBookBackend.Repository.IRepository;
using MyBookBackend.Service.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace MyBookBackend.Service
{
    public class AuthService : IAuthService
    {
        public readonly IAuthRepository _authRepository;
        public readonly IUserRepository _userRepository;
        private readonly IConfiguration  _config;
        private readonly  IHttpContextAccessor _httpContextAccessor;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        public string GenerateToken(User user)

        {
            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.Role, user.Role.RoleName),
new Claim(ClaimTypes.Name, user.Email),
new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

            };
            var key =
       _config["Jwt:Key"]
       ?? throw new Exception("JWT Key Missing");
            var securityKey =
    new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<Result<LoggedInUserDto>> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email)
               || string.IsNullOrEmpty(password))
            {
                return Result<LoggedInUserDto>
                    .Failure("Invalid request");
            }

            var user =
                await _userRepository
                    .GetUserByEmail(email);

            if (user == null)
            {
                return Result<LoggedInUserDto>
                    .Failure("User not found");
            }

            bool isValid =
       BCrypt.Net.BCrypt.Verify(
           password,
           user.Password);

            if (!isValid)
            {
                return Result<LoggedInUserDto>
                    .Failure(
                        "Username or password incorrect");
            }

            // BUSINESS LOGIC HERE
            var accessToken =
                GenerateToken(user);

            var refreshToken =
                GenerateRefreshToken();

            user.RefreshToken =
                refreshToken;

            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(7);

            await _authRepository.Update(user);

            var dto = new LoggedInUserDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Result<LoggedInUserDto>
                .Success(dto,
                    "User logged in successfully");
        }
        public async Task<Result<LoggedInUserDto>> RefreshToken(string refreshToken)
        {
            var user =
                await _authRepository
                    .GetUserByRefreshToken(refreshToken);

            if (user == null)
            {
                return Result<LoggedInUserDto>
                    .Failure("Invalid refresh token");
            }

            if (user.RefreshTokenExpiryTime
                <= DateTime.UtcNow)
            {
                return Result<LoggedInUserDto>
                    .Failure("Refresh token expired");
            }

            var newAccessToken =
               GenerateToken(user);

            var newRefreshToken =
              GenerateRefreshToken();

            user.RefreshToken =
                newRefreshToken;

            user.RefreshTokenExpiryTime =
                DateTime.UtcNow.AddDays(7);

            await _authRepository.Update(user);

            return Result<LoggedInUserDto>
                .Success(new LoggedInUserDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
        }

        public async Task Logout(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            user.RefreshToken = null;

            user.RefreshTokenExpiryTime = null;

            await _authRepository.Update(user);
        }



        public int getLoggedInUserId
        {
            get
            {
                return int.Parse(
                    _httpContextAccessor
                        .HttpContext!
                        .User
                        .FindFirst(
                            ClaimTypes.NameIdentifier)!
                        .Value);
            }
        }

    }
}
