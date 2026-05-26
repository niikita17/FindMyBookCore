using MyBook_Backend.Models.DomainModels;
using MyBook_Backend.Models.DTO;
using MyBook_Backend.Repository.IRepository;
using MyBook_Backend.Services.IServices;

namespace MyBook_Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result<UserResponseDto>> AddUserByAdmin(RegisterUserDto model)
        {

            if (model == null)
            {
                return Result<UserResponseDto>
                    .Failure("Invalid request");
            }

            var exists = await _userRepository.GetUserByEmail(model.Email);

            if (exists!=null)
            {
                return Result<UserResponseDto>
                    .Failure("User already exists");
            }


            var hashedPassword =
         BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                MobileNo = model.MobileNo,
                RoleId = model.RoleId,
                Password = hashedPassword
            };

            await _userRepository.RegisterUser(user);
            var response = new UserResponseDto
            {
         
                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId
            };
            return Result<UserResponseDto>
           .Success(response,
           "User registered successfully");


        }

        public async Task<Result<UserResponseDto>> AddUser(RegisterUserDto model)
        {

            if (model == null)
            {
                return Result<UserResponseDto>
                    .Failure("Invalid request");
            }

            var exists = await _userRepository.GetUserByEmail(model.Email);

            if (exists != null)
            {
                return Result<UserResponseDto>
                    .Failure("User already exists");
            }


            var hashedPassword =
         BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                MobileNo = model.MobileNo,
                RoleId = 2,
                Password = hashedPassword
            };

            await _userRepository.RegisterUser(user);
            var response = new UserResponseDto
            {

                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId
            };
            return Result<UserResponseDto>
           .Success(response,
           "User registered successfully");


        }

        public async Task<Result<UserResponseDto>> DeleteUser(int Id)
        {
            if (Id == null)
                return Result<UserResponseDto>.Failure("Id is null");
          var user=  await _userRepository.DeleteUser(Id);
            if(user==null)
                return Result<UserResponseDto>.Failure("something went wrong");
            var response = new UserResponseDto
            {

                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId
            };

            return Result<UserResponseDto>.Success(response, "User Deleted Sucessfully");
        }

        public async Task<Result<UserResponseDto>> EditUser(int id,UserResponseDto model)
        {
            if (id == null)
                return Result<UserResponseDto>.Failure("Id is null");
            User user =await  _userRepository.EditUser(id, model);
            var response = new UserResponseDto
            {

                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId
            };
            return Result<UserResponseDto>.Success(response, "updated sucessfully");


        }

        public  async Task<Result<UserResponseDto>> GetUserByEmail(string Email)
        {
            if (Email == null)
                return Result<UserResponseDto>.Failure("email is null");
            var user= await _userRepository.GetUserByEmail(Email);
            if (user == null)
                throw new ArgumentNullException();
            var response = new UserResponseDto
            {

                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId
            };
            return Result<UserResponseDto>.Success(response, "User found");


        }

        public async Task<Result<UserResponseDto>> GetUserById(int Id)
        {
            if (Id == null)
                return Result<UserResponseDto>.Failure("Id is null");

            User user = await _userRepository.GetUserById(Id);
            var response = new UserResponseDto
            {

                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId
            };
            return Result<UserResponseDto>.Success(response, "user found sucessfully");

        }

    

        public async Task<Result<List<UserResponseDto>>> GetAll()
        {
            var users=await _userRepository.GetAll();
            var res = users.Select(user => new UserResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                RoleId = user.RoleId

            }).ToList();

            return Result<List<UserResponseDto>>.Success(res, "sucess");
        }
    }
}
