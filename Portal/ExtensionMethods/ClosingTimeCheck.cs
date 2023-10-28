using Portal.Models;
using System.ComponentModel.DataAnnotations;

namespace Portal.ExtensionMethods
{
    public class ClosingTimeCheck : ValidationAttribute
    {
        public ClosingTimeCheck()
        {
            ErrorMessage = "Please choose a reasonable closing time that is at least one hour from pickup time";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {

            if (value is TimeSpan time)
            {
                var model = (PackageModel)validationContext.ObjectInstance;
                if (time >= TimeSpan.FromHours(7) && time <= TimeSpan.FromHours(23) && time > (model.PickUpTime + TimeSpan.FromMinutes(59)))
                {
                    return ValidationResult.Success!;
                }
            }


            return new ValidationResult(ErrorMessage);
        }
    }
}
