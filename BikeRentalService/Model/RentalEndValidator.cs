using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{
    public class RentalEndValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rental = (Rental)validationContext.ObjectInstance;
            if (rental.RentalBegin >= rental.RentalEnd)
            {
                return new ValidationResult("Rental end must be after rental start!");
            }
            return ValidationResult.Success;
        }
    }
}
