using Core.Domain.Enumerations;
using Portal.ExtensionMethods;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class StudentRegisterModel
    {
        [Key]
        [Display(Name = "Student ID")]
        [Required(ErrorMessage = "Fill out your student ID")]
        public string? StudentID { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Fill out your first name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Fill out your last name")]
        public string? LastName { get; set; }

        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = "Fill out your date of birth")]
        [AgeCheck(16, ErrorMessage = "You have to be at least 16")]
        public DateTime DOB { get; set; }

        [Display(Name = "E-mailaddress")]
        [Required(ErrorMessage = "Fill out your e-mailaddress")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Select the city where you study")]
        public CityEnum City { get; set; }

        [Display(Name = "Phonenumber")]
        [Required(ErrorMessage = "Fill out your phone number")]
        public string? Phone { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Choose a password")]
        public string? Password { get; set; }
    }
}
