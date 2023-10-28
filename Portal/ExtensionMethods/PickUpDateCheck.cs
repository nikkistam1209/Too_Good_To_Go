using System.ComponentModel.DataAnnotations;

namespace Portal.ExtensionMethods
{
    public class PickUpDateCheck : ValidationAttribute
    {
        public PickUpDateCheck()
        {
            ErrorMessage = "Please fill out a date between today and two days from now";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value is DateTime date)
            {
                if (date >= DateTime.Today && date <= DateTime.Today.AddDays(2))
                {
                    return ValidationResult.Success;
                }
            }


            return new ValidationResult(ErrorMessage);
        }
    }
}
