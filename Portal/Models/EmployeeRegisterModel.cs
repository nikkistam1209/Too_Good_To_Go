using Core.Domain.Entities;
using Core.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class EmployeeRegisterModel
    {
        [Key]
        [Display(Name = "Employee ID")]
        [Required(ErrorMessage = "Please enter your employee ID")]
        public string? EmployeeID { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string? LastName { get; set; }

        [Display(Name = "Canteen")]
        [Required(ErrorMessage = "Please select the canteen where you work")]
        public CanteenEnum? WorkPlace { get; set; }

        public IEnumerable<Canteen>? CanteenList { get; set; }

        [Display(Name = "Password")]
        [PasswordValidationCheck]
        public string? Password { get; set; }

        [Display(Name = "Repeat password")]
        [PasswordCheck("Password")]
        public string? PasswordReEntry { get; set; }
    }
}
