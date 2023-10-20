using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class LoginModel
    {
        [Display(Name = "Student/employee ID")]
        [Required(ErrorMessage = "Please fill out your ID")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Please fill out your password")]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
