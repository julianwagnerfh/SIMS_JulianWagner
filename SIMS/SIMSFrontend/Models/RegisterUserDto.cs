using System.ComponentModel.DataAnnotations;

namespace SIMSFrontend.Models
{
    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
