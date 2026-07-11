using MyBookBackend.Domain.DomainModels;
using MyBookBackend.Common.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MyBookBackend.Repository.IRepository
{
    public interface IUserRepository
    {



        public  Task<User> GetUserByEmail(string email);
        public Task<User> GetUserById(int id);

        public  Task<User> RegisterUser(User model);
        public Task<User> EditUser(int id, UserResponseDto user);

        public Task<User> DeleteUser(int id);

        public Task<List<User>> GetAll();





    }
}
