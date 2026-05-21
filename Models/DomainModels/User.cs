using System.ComponentModel.DataAnnotations;

namespace MyBook_Backend.Models.DomainModels
{
    public class User
    {
       public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(6)]
        public string Password {  get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public  string MobileNo { get; set; }
        [Required]
        public int RoleId {  get; set; }
        public Role Role { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
