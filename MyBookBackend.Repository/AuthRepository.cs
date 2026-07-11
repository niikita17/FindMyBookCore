using Microsoft.EntityFrameworkCore;

using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;
using MyBookBackend.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using MyBookBackend.Domain.Data;

namespace MyBookBackend.Repository
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
            _dbContext = dbContext;
        }



        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();
        }
        public async Task<User?>
    GetUserByRefreshToken(string refreshToken)
        {
            return await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken == refreshToken);
        }

   

    }
}
