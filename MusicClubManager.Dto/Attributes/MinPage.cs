using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Dto.Attributes
{
    public class MinPage(uint minPage) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Null values are handled by RequiredAttribute or can be checked separately
            }

            if (value is not uint page)
            {
                return new ValidationResult("Page must be a positive integer.");
            }

            if (page < minPage)
            {
                return new ValidationResult($"Minimum page allowed is {minPage}.");
            }

            return ValidationResult.Success;
        }
    }
}
