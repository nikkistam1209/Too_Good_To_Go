using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class PasswordValidationCheck : ValidationAttribute
{
    public string LengthErrorMessage { get; set; } = "The password must be at least 8 characters long";
    public string UppercaseErrorMessage { get; set; } = "The password must contain at least one uppercase letter";
    public string LowercaseErrorMessage { get; set; } = "The password must contain at least one lowercase letter";
    public string DigitErrorMessage { get; set; } = "The password must contain at least one digit";
    public string SpecialCharacterErrorMessage { get; set; } = "The password must contain at least one special character";

    public PasswordValidationCheck()
    {
        ErrorMessage = "Please choose a password";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string password)
        {
            if (IsPasswordValid(password))
            {
                return ValidationResult.Success;
            }
        }
        string errorMessage = ErrorMessage ?? "Please choose a password";

        return new ValidationResult(errorMessage);
    }


    private bool IsPasswordValid(string password)
    {
        bool isPasswordValid = true;

        if (password.Length < 8)
        {
            ErrorMessage = LengthErrorMessage;
            isPasswordValid = false;
        }
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            ErrorMessage = UppercaseErrorMessage;
            isPasswordValid = false;
        }
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            ErrorMessage = LowercaseErrorMessage;
            isPasswordValid = false;
        }
        if (!Regex.IsMatch(password, @"\d"))
        {
            ErrorMessage = DigitErrorMessage;
            isPasswordValid = false;
        }
        if (!Regex.IsMatch(password, @"[@$!%*?&]"))
        {
            ErrorMessage = SpecialCharacterErrorMessage;
            isPasswordValid = false;
        }

        return isPasswordValid;
    }
}
