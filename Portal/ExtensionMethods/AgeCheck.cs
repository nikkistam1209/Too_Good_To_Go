using System;
using System.ComponentModel.DataAnnotations;

namespace Portal.ExtensionMethods
{
    public class AgeCheck : ValidationAttribute
    {
        private readonly int _minAge;

        public AgeCheck(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || !(value is DateTime dateOfBirth))
            {
                return new ValidationResult("Please enter your date of birth");
            }

            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }

            if (age < _minAge)
            {
                return new ValidationResult("You have to be at least 16 years old");
            }

            return ValidationResult.Success;
        }
    }
}
