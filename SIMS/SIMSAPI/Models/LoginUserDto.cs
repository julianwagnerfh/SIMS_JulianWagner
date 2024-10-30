using System.ComponentModel.DataAnnotations;

namespace SIMSAPI.Models
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Benutzername ist erforderlich.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Passwort ist erforderlich.")]
        public string Password { get; set; }
    }
}
