using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.ExtensionMethods;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class PackageModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please give the package a descriptive name")]
        public string? Name { get; set; }

        [Required]
        [PickUpDateCheck]
        [Display(Name = "Pick Up Date")]
        public DateTime PickUpDate { get; set; }

        [Required]
        [PickUpTimeCheck]
        [Display(Name = "Pick Up Time")]
        public TimeSpan PickUpTime { get; set; }

        [Required]
        [ClosingTimeCheck]
        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        [Required]
        [PriceCheck]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please indicate the type of package")]
        public PackageEnum? Type { get; set; }

        public Student? StudentReservation { get; set; }

        public bool MyCanteenOffersDinners { get; set; }

        // available products
        public IEnumerable<SelectListItem>? AvailableProducts { get; set; } = new List<SelectListItem>();

        // selected products
        [Required]
        [ProductsCheck]
        [Display(Name = "Select Products")]
        public List<int> SelectedProductIds { get; set; } = new List<int>();


    }
}
