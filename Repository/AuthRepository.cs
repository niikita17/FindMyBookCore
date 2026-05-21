using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyBook_Backend.Repos
{
    public class AuthRepository : IAuthRepository
    {
        public readonly IConfiguration _config;
        public readonly IUserRepository _userRepository;
        public readonly ApplicationDbContext _dbContext;
        public AuthRepository(IConfiguration config, IUserRepository userRepository, ApplicationDbContext dbContext)
        {
            _config= config;
            _userRepository = userRepository;
            dbContext = _dbContext;
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
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())

            };
            var key = new SymmetricSecurityKey(
           Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
            
        public async Task<LoggedInUserDto> Login(string email, string password)
        {
            
            var user = await _userRepository.GetUserByEmail(email);
         

            var token = GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            user.RefreshTokenExpiryTime =
                DateTime.Now.AddDays(7);

            await _dbContext.SaveChangesAsync();
            return new LoggedInUserDto
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };


        }
    }
}
