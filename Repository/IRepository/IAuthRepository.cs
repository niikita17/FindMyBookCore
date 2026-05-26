using Microsoft.AspNetCore.Mvc;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;


namespace MyBook_Backend.Repository.IRepository
{
    public interface IAuthRepository
    {
         
        public  Task Update(User user);


       public Task<User?> GetUserByRefreshToken(
    string refreshToken);

    


    }
}
