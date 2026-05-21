using Microsoft.EntityFrameworkCore;
using MyBook_Backend.Data;
using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;

namespace MyBook_Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _dbContext;


        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUserByEmail(string email)
        {


            var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User> RegisterUser(User model)
        {

      await _dbContext.Users.AddAsync(model);
        await    _dbContext.SaveChangesAsync();


           return model;
          
        }

        public async Task<User> EditUser(int id, UserResponseDto model)
        {
            
            var existingUser = await _dbContext.Users.FindAsync(id);

            if (existingUser == null)
                return null;

            // Update fields
            existingUser.MobileNo = model.MobileNo;
            existingUser.Name = model.Name;
            existingUser.Email = model.Email;
    
            await _dbContext.SaveChangesAsync();
            return existingUser; 
        }

        //delete= user
        public async Task<User> DeleteUser(int id)
        {
            var existingUser = await _dbContext.Users.FindAsync(id);

            if (existingUser == null)
                return null;
             _dbContext.Users.Remove(existingUser);

            await _dbContext.SaveChangesAsync();


            return existingUser;
        }

        public async Task<User> GetUserById(int id)
        {
            User user = await _dbContext.Users.FindAsync(id);
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            var user = await _dbContext.Users.ToListAsync();

            return user;
        }
    }
}
