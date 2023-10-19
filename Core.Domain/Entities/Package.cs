using Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    public class Package
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please give the package a descriptive name")]
        public string? Name { get; set; }


        public CanteenEnum? Canteen { get; set; }

        [Required(ErrorMessage = "Please indicate the date and time when the package can be picked up")]
        public DateTime PickUp { get; set; }

        [Required(ErrorMessage = "Please indicate until what time the package can be picked up")]
        public DateTime ClosingTime { get; set; }

        public bool AgeRestriction { get; set; }

        [Required(ErrorMessage = "Please give the package a fitting price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please indicate the type of package")]
        public PackageEnum? Type { get; set; }

        public Student? StudentReservation { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();


    }
}
