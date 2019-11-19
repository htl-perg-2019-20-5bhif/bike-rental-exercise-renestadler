using BikeRentalService.Model;
using System;

namespace BikeRentalService
{

    public class CostCalculator
    {
        //Calculates the number of hours first
        //Check if the time is smaller than 15 minutes --> 0
        //Calculate price of first hour + price for additional hours -->  value
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
