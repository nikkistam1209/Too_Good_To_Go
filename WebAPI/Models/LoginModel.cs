using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please fill out your ID")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Please fill out your password")]
        public string? Password { get; set; }

    }
}
