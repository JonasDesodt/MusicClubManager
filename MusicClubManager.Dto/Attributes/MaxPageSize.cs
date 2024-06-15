using System.ComponentModel.DataAnnotations;

namespace MusicClubManager.Dto.Attributes
{
    public class MaxPageSize(uint maxPageSize) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Null values are handled by RequiredAttribute or can be checked separately
            }

            if (value is not uint pageSize)
            {
                return new ValidationResult("Page size must be a positive integer.");
            }

            if (pageSize > maxPageSize)
            {
                return new ValidationResult($"Maximum page size allowed is {maxPageSize}.");
            }

            return ValidationResult.Success;
        }
    }
}