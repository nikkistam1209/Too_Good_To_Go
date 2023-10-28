using System.ComponentModel.DataAnnotations;

namespace Portal.ExtensionMethods
{
    public class PriceCheck : ValidationAttribute
    {
        public PriceCheck()
        {
            ErrorMessage = "Please give the package a fitting price";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is decimal price)
            {
                if (price > 0 && price < 200)
                {
                    return ValidationResult.Success!;
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
