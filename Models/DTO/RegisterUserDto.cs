using MyBook_Backend.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace MyBook_Backend.Models.DTO
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
