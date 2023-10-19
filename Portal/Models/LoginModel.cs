using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class LoginModel
    {
        [Display(Name = "Student/employee ID")]
        [Required(ErrorMessage = "Student/employee ID is required!")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
