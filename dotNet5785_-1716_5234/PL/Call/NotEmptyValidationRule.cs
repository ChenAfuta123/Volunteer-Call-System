using System.Globalization;
using System.Windows.Controls;

namespace PL.Call
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "השדה לא יכול להיות ריק");
            }
            return ValidationResult.ValidResult;
        }
    }
}
