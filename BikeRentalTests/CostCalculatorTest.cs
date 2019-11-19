using BikeRentalService;
using BikeRentalService.Model;
using System;
using Xunit;

namespace BikeRentalTests
{
    public class CostCalculatorTest
    {
        [Fact]
        public void Calculate10Minutes()
        {
            CostCalculator costCalculator = new CostCalculator();
            Bike bike = new Bike() { BikeId = 0, Brand = "", BikeCategory = "Mountainbike", PurchaseDate = DateTime.Now, RentalPriceFirstHour = 10, RentalPriceAdditionalHour = 15 };
            Rental rental = new Rental() { Bike = bike, RentalBegin = DateTime.Now, RentalEnd = DateTime.Now.AddMinutes(10) };

            var result = costCalculator.CalculateTotalCost(rental);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Calculate15Minutes()
        {
            CostCalculator costCalculator = new CostCalculator();
            Bike bike = new Bike() { BikeId = 0, Brand = "", BikeCategory = "Mountainbike", PurchaseDate = DateTime.Now, RentalPriceFirstHour = 10, RentalPriceAdditionalHour = 15 };
            Rental rental = new Rental() { Bike = bike, RentalBegin = DateTime.Now, RentalEnd = DateTime.Now.AddMinutes(15) };

            var result = costCalculator.CalculateTotalCost(rental);

            Assert.Equal(10, result);
        }

        [Fact]
        public void Calculate80Minutes()
        {
            CostCalculator costCalculator = new CostCalculator();
            Bike bike = new Bike() { BikeId = 0, Brand = "", BikeCategory = "Mountainbike", PurchaseDate = DateTime.Now, RentalPriceFirstHour = 10, RentalPriceAdditionalHour = 15 };
            Rental rental = new Rental() { Bike = bike, RentalBegin = DateTime.Now, RentalEnd = DateTime.Now.AddMinutes(80) };

            var result = costCalculator.CalculateTotalCost(rental);

            Assert.Equal(25, result);
        }

        [Fact]
        public void Calculate135Minutes()
        {
            CostCalculator costCalculator = new CostCalculator();
            Bike bike = new Bike() { BikeId = 0, Brand = "", BikeCategory = "Mountainbike", PurchaseDate = DateTime.Now, RentalPriceFirstHour = 10, RentalPriceAdditionalHour = 15 };
            Rental rental = new Rental() { Bike = bike, RentalBegin = DateTime.Now, RentalEnd = DateTime.Now.AddMinutes(135) };

            var result = costCalculator.CalculateTotalCost(rental);

            Assert.Equal(40, result);
        }
    }
}
