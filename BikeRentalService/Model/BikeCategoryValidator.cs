using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{

    class BikeCategoryValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bike = (Bike)validationContext.ObjectInstance;
            string[] categories = { "Standard bike", "Mountainbike", "Trecking bike", "Racing bike" };
            foreach (string category in categories)
            {
                if (category.Equals(bike.BikeCategory))
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("This bike category is not allowed. Try 'Standard bike', 'Mountainbike', 'Trecking bike' or 'Racing bike'.");
        }
    }
}
