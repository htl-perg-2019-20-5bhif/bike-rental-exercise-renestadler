using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalService.Model
{
    public class Bike
    {
        public int BikeId { get; set; }

        [Required]
        [StringLength(25)]
        public string Brand { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PurchaseDate { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfLastService { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        [Required]
        public double RentalPriceFirstHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, double.MaxValue)]
        [Required]
        public double RentalPriceAdditionalHour { get; set; }

        [BikeCategoryValidator]
        public string BikeCategory { get; set; }
    }
}
