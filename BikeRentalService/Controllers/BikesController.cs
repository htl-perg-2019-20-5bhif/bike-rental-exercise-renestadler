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
    public class BikesController : ControllerBase
    {
        private readonly BikeRentalContext _context;

        public BikesController(BikeRentalContext context)
        {
            _context = context;
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes([FromQuery] string sortCriteria)
        {
            if (sortCriteria == null)
            {
                return await _context.Bikes.ToListAsync();
            }
            else if (sortCriteria.ToLower().Equals("firsthour"))
            {
                return (await _context.Bikes.OrderBy(p => p.RentalPriceFirstHour).ToListAsync());
            }
            else if (sortCriteria.ToLower().Equals("additionalhour"))
            {
                return (await _context.Bikes.OrderBy(p => p.RentalPriceAdditionalHour).ToListAsync());
            }
            else if (sortCriteria.ToLower().Equals("purchasedate"))
            {
                return (await _context.Bikes.OrderByDescending(p => p.PurchaseDate).ToListAsync());
            }
            else
            {
                return StatusCode(400, "Filter option not available. Try firsthour, additionalhour or purchasedate instead.");
            }
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);

            if (bike == null)
            {
                return NotFound();
            }

            return bike;
        }


        // PUT: api/Bikes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, Bike bike)
        {
            if (id != bike.BikeId)
            {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
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

        // POST: api/Bikes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.BikeId }, bike);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bike>> DeleteBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            // It must not be possible to delete a bike if one or more rentals exist for it.
            if ((await _context.Rentals.ToListAsync()).Any(r => r.BikeId == id && r.RentalEnd == DateTime.MinValue))
            {
                return StatusCode(400, "Cannot delete a bike that is currently on a rental.");
            }
            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return bike;
        }

        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.BikeId == id);
        }
    }
}
