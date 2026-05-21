using MyBook_Backend.Models.DomainModels;

namespace MyBook_Backend.Models.DTO
{
    public class LoggedInUserDto
    {

        public string Name { get; set; }


        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
