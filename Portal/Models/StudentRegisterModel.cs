using Core.Domain.Enumerations;
using Portal.ExtensionMethods;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class StudentRegisterModel
    {
        [Key]
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Please enter your student ID")]
        public string? StudentID { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string? LastName { get; set; }

        [Display(Name = "Date of birth")]
        [AgeCheck(16)]
        public DateTime? DOB { get; set; }

        [Display(Name = "E-mailaddress")]
        [Required(ErrorMessage = "Please enter your e-mailaddress")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please select a city")]
        public CityEnum? City { get; set; }

        [Display(Name = "Phonenumber")]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string? Phone { get; set; }

        [Display(Name = "Password")]
        [PasswordValidationCheck]
        public string? Password { get; set; }

        [Display(Name = "Repeat password")]
        [PasswordCheck("Password")]
        public string? PasswordReEntry { get; set; }
    }
}
