using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{
    public class IsPaidValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rental = (Rental)validationContext.ObjectInstance;
            if (rental.RentalEnd == DateTime.MinValue && rental.Paid)
            {
                return new ValidationResult("Ride must have ended before you can pay!");
            }
            return ValidationResult.Success;
        }
    }
}
