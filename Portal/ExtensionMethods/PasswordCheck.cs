using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class PasswordCheck : ValidationAttribute
{
    private readonly string _propertyToMatch;

    public PasswordCheck(string propertyToMatch)
    {
        _propertyToMatch = propertyToMatch;
        ErrorMessage = "Passwords do not match";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var propertyInfo = validationContext.ObjectType.GetProperty(_propertyToMatch);
        if( propertyInfo != null) {
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (object.Equals(value, propertyValue))
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage);
    }
}
