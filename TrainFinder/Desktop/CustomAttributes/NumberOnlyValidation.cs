using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Desktop
{
    public class NumberOnlyValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;
           
            if(!double.TryParse(text,out double result))
                return new ValidationResult("Numbers Only");
            return ValidationResult.Success;
        }
    }
}
