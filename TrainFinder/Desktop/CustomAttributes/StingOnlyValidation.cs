using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Desktop
{
    public class StingOnlyValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;
            if (text.Any(char.IsDigit))
                return new ValidationResult("No Numbers Allowed");
            return ValidationResult.Success;
        }
    }
}
