using Core.Domain.Entities;
using Core.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class EmployeeRegisterModel
    {
        [Key]
        [Display(Name = "Employee ID")]
        [Required(ErrorMessage = "Fill out your employee ID")]
        public string? EmployeeID { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Fill out your first name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Fill out your last name")]
        public string? LastName { get; set; }

        [Display(Name = "Canteen")]
        [Required(ErrorMessage = "Select the canteen where you work")]
        public CanteenEnum WorkPlace { get; set; }

        public IEnumerable<Canteen>? CanteenList { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Choose a password")]
        public string? Password { get; set; }
    }
}
