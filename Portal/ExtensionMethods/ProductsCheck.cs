using System.ComponentModel.DataAnnotations;

namespace Portal.ExtensionMethods
{
    public class ProductsCheck : ValidationAttribute
    {
        public ProductsCheck()
        {
            ErrorMessage = "Please select at least one product";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is List<int> selectedProductIds && selectedProductIds.Any())
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
