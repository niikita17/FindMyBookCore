using MyBook_Backend.Models.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace MyBook_Backend.Models.DTO
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
   
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string RoleName { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }


    }
}
