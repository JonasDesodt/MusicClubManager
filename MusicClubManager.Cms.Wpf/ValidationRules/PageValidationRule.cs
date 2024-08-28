using System.Globalization;
using System.Windows.Controls;

namespace MusicClubManager.Cms.Wpf.ValidationRules
{
    public class PageValidationRule : ValidationRule
    {
        public int TotalPages { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse((string)value, out int page)) return new ValidationResult(false, "The page must be a number");

            if (page < 1 || page > TotalPages) return new ValidationResult(false, $"The page cannot be lower than 1 or higher than {TotalPages}");

            return ValidationResult.ValidResult; 
        }
    }
}
