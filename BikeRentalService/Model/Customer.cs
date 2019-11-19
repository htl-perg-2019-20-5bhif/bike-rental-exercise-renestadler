using BikeRentalService.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalService.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }

        //Possible Values:
        //Male, Female, Unknown
        [GenderValidator]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(75)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDay { get; set; }

        [Required]
        [StringLength(75)]
        public string Street { get; set; }

        [StringLength(10)]
        public string HouseNr { get; set; }

        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(75)]
        public string Town { get; set; }
    }
}
