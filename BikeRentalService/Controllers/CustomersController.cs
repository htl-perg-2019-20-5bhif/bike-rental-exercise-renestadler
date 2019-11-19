using BikeRentalService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeRentalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly BikeRentalContext _context;

        public CustomersController(BikeRentalContext context)
        {
            _context = context;
        }

        // GET: api/Customers/<filter>
        // All Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers([FromQuery] string filter)
        {
            if (filter == null)
            {
                return await _context.Customers.ToListAsync();
            }
            return await _context.Customers.Where(c => c.FirstName.Contains(filter)).ToListAsync();
        }

        // GET: api/Customers/Rentals/<id>
        // All Rentals for one Customer
        [HttpGet("rentals/{id}")]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return await _context.Rentals.Where(r => r.CustomerId == id).ToListAsync();
        }

        // GET: api/Customers/<id>
        // One Customer
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/<id>
        // Update Customer
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // Add new Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/<id>
        // Delete Customer Including all it's Rentals
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.RemoveRange(_context.Rentals.Where(r => r.CustomerId == id).ToList());
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }


        // Check if Customer Exists
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
