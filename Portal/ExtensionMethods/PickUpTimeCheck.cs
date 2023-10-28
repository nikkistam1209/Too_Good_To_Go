using System.ComponentModel.DataAnnotations;

namespace Portal.ExtensionMethods
{
    public class PickUpTimeCheck : ValidationAttribute
    {
        public PickUpTimeCheck()
        {
            ErrorMessage = "Please choose a reasonable pickup time";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value is TimeSpan time)
            {
                if (time >= TimeSpan.FromHours(6) && time <= TimeSpan.FromHours(22))
                {
                    return ValidationResult.Success;
                }
            }


            return new ValidationResult(ErrorMessage);
        }
    }
}
