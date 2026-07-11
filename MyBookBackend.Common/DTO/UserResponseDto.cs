using System.ComponentModel.DataAnnotations;

namespace MyBookBackend.Common.DTO
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
       
        public string RoleName { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }


    }
}
