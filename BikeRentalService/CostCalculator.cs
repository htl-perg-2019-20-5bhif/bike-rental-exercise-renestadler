using BikeRentalService.Model;
using System;

namespace BikeRentalService
{

    public class CostCalculator
    {
        public double CalculateTotalCost(Rental rent)
        {
            TimeSpan? timeDifference = (rent.RentalEnd - rent.RentalBegin);
            var totalMinutes = timeDifference.Value.TotalMinutes;
            var startedHours = (int)Math.Ceiling((totalMinutes) / 60);

            return totalMinutes <= 15 ? 0 : rent.Bike.RentalPriceFirstHour +
                (rent.Bike.RentalPriceAdditionalHour * (startedHours - 1));
        }
    }
}
