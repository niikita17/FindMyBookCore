using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;


namespace MyBook_Backend.Repository.IRepository
{
    public interface IAuthRepository
    {
         string GenerateRefreshToken();
         string GenerateToken(User user);
        public Task<LoggedInUserDto>   Login(string email, string password);


            
            
            
            
    }
}
