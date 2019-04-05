using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Desktop
{
    internal static class Validations
    {
        public static bool NullEmptyStringValidation(string stringText)
        {
            return string.IsNullOrWhiteSpace(stringText) || string.IsNullOrEmpty(stringText);
        }

        public static bool DigitCheck(string number)
        {
            bool re =!float.TryParse(number, out float res);
            Debug.WriteLine(re);
            return re;
        }

        public static bool HasNumber(string text)
        {
            return text.Any(char.IsNumber);
        }
    }
}
