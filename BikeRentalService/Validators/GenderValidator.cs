using BikeRentalService.Model;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Validators
{

    class GenderValidator : ValidationAttribute
    {
        //Validates if the given gender is valid
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;
            string[] genders = { "Male", "Female", "Unknown" };
            foreach (string gender in genders)
            {
                if (gender.Equals(customer.Gender))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("This Gender is not allowed. Try 'Male', 'Female' or 'Unknown'.");
        }
    }
}
