using System.ComponentModel.DataAnnotations;

namespace MyBookBackend.Common.DTO
{
    public class RegisterUserDto
    {

        public string Name { get; set; }
  
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        public string MobileNo { get; set; }
       
        


    }
}
