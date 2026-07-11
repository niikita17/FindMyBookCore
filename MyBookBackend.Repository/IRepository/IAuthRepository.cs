using Microsoft.AspNetCore.Mvc;
using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;


namespace MyBookBackend.Repository.IRepository
{
    public interface IAuthRepository
    {
         
        public  Task Update(User user);


       public Task<User?> GetUserByRefreshToken(
    string refreshToken);

    


    }
}
