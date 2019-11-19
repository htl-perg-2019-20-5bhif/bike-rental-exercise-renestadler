using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalService.Model
{
    public class Rental
    {
        public int RentalId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public int BikeId { get; set; }

        public Bike Bike { get; set; }

        [Required]
        public DateTime RentalBegin { get; set; }

        [RentalEndValidator]
        public DateTime RentalEnd { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        [Required]
        public double Total { get; set; }

        [Required]
        public Boolean Paid { get; set; }
    }
}