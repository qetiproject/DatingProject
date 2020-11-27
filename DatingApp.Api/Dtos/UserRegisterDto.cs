using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage ="You must specify password between 6 and 10 characters")]
        public string Password { get; set; }
    }
}
