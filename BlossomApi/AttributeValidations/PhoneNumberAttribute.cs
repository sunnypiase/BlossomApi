using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlossomApi.AttributeValidations
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Invalid phone number format.";
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);

        public PhoneNumberAttribute() : base(DefaultErrorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || PhoneNumberRegex.IsMatch(value.ToString()))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
        }
    }
}