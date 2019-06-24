using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.CustomAttributes
{
    public class PhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;

            if (text.Any(char.IsLetter))
                return new ValidationResult("Numbers Only");
            if(text.Length!=10)
                return new ValidationResult("Phone Number Should Contain 10 Digits");
            return ValidationResult.Success;
        }
    }
}