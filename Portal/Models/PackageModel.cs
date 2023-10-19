using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class PackageModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please give the package a descriptive name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please indicate the date when the package can be picked up")]
        [Display(Name = "Pick Up Date")]
        public DateTime PickUpDate { get; set; }

        [Required(ErrorMessage = "Please indicate the time when the package can be picked up")]
        [Display(Name = "Pick Up Time")]
        public TimeSpan PickUpTime { get; set; }

        [Required(ErrorMessage = "Please indicate until what time the package can be picked up")]
        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        [Required(ErrorMessage = "Please give the package a fitting price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please indicate the type of package")]
        public PackageEnum? Type { get; set; }

        public Student? StudentReservation { get; set; }

        // available products
        public IEnumerable<SelectListItem>? AvailableProducts { get; set; } = new List<SelectListItem>();
        //public IList<string>? SelectedProducts { get; set; } = new List<string>();

        // selected products
        [Required(ErrorMessage = "Please select at least one product")]
        public ICollection<Product> SelectedProducts { get; set; } = new List<Product>();


        [Required(ErrorMessage = "Please select at least one product")]
        [Display(Name = "Select Products")]
        public List<int> SelectedProductIds { get; set; }
    }
}
