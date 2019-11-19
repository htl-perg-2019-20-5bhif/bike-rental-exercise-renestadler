using BikeRentalService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly BikeRentalContext _context;

        public RentalsController(BikeRentalContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("start")]
        public async Task<ActionResult> StartRental([FromQuery] int customerId, [FromQuery] int bikeId)
        {
            if (!(await _context.Customers.ToListAsync()).Any(c => c.CustomerId == customerId) ||
                !(await _context.Bikes.ToListAsync()).Any(b => b.BikeId == bikeId))
            {
                return StatusCode(404, "Unable to create rental. Please check the customerId and the bikeId you entered.");
            }

            // Check if this customer already has an active rental
            if ((await _context.Rentals.ToListAsync()).Any(r => r.CustomerId == customerId &&
            r.RentalBegin != DateTime.MinValue && r.RentalEnd == DateTime.MinValue))
            {
                return StatusCode(400, "This customer already has an active rental.");
            }

            Rental rent = new Rental();

            rent.Customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (rent.Customer == null)
            {
                return StatusCode(404, "The customer could not be found.");
            }

            rent.Bike = _context.Bikes.FirstOrDefault(b => b.BikeId == bikeId);
            if (rent.Bike == null)
            {
                return StatusCode(404, "The bike could not be found.");
            }

            // Assign the values
            rent.RentalBegin = DateTime.Now;
            rent.Total = 0;
            rent.Paid = false;

            _context.Rentals.Add(rent);
            _context.SaveChanges();

            return StatusCode(200, rent);
        }

        [HttpPut]
        [Route("end/{rentalId}")]
        public async Task<ActionResult> EndRental(int rentalId)
        {
            Rental rentToEnd = _context.Rentals.Include(r => r.Bike).ToList().FirstOrDefault(r => r.RentalId == rentalId);
            if (rentToEnd == null)
            {
                return StatusCode(404, "The rental could not be found.");
            }

            // Check if it was already ended
            if (rentToEnd.RentalEnd != DateTime.MinValue)
            {
                return StatusCode(400, "The rental has already been ended.");
            }

            // Assign the values
            rentToEnd.RentalEnd = DateTime.Now;
            CostCalculator costCalculator = new CostCalculator();
            rentToEnd.Total = costCalculator.CalculateTotalCost(rentToEnd);

            _context.SaveChanges();

            return StatusCode(200, rentToEnd);
        }

        // Put: api/Rentals
        [HttpPut]
        [Route("pay/{rentalId}")]
        public async Task<ActionResult> SetPaid(int rentalId)
        {

            var rent = _context.Rentals.FirstOrDefault(r => r.RentalId == rentalId);

            if (rent.Total > 0 || rent.RentalEnd != DateTime.MinValue)
            {
                rent.Paid = true;
            }
            else
            {
                return StatusCode(400, "The total costs have not been calculated.");
            }

            _context.SaveChanges();

            return StatusCode(200, "The rental has been paid.");
        }

        /// <summary>
        /// Returns a List of all unpaid rentals (where TotalCosts > 0 and the rental end has been set)
        /// </summary>
        /// <returns>List of all unpaid rentals</returns>
        [HttpGet]
        [Route("unpaid")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetUnpaid()
        {
            return StatusCode(200, _context.Rentals.Where(r => !r.Paid && r.RentalEnd != DateTime.MinValue)
                .Include(r => r.Customer).Include(r => r.Bike).ToList().SelectMany(rental =>
                    new RentalDto[] {
                        new RentalDto{
                            CustomerId = rental.Customer.CustomerId,
                            FirstName = rental.Customer.FirstName,
                            LastName = rental.Customer.LastName,
                            RentalId = rental.RentalId,
                            RentalBegin = rental.RentalBegin,
                            RentalEnd = rental.RentalEnd,
                            TotalCost = rental.Total
                        }
                    }));
        }

        // GET: api/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            return await _context.Rentals.Include(r => r.Customer).Include(r => r.Bike).ToListAsync();
        }

        class RentalDto
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int RentalId { get; set; }
            public DateTime RentalBegin { get; set; }
            public DateTime RentalEnd { get; set; }
            public double TotalCost { get; set; }
        }
    }
}
