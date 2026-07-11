
namespace MyBookBackend.Common.DTO
{
    public class LoggedInUserDto
    {

        public string Name { get; set; }


        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
