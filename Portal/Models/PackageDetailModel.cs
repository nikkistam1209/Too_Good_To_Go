using Core.Domain.Entities;
using Core.Domain.Enumerations;

namespace Portal.Models
{
    public class PackageDetailModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }


        public DateTime PickUpDate { get; set; }

        public DateTime ClosingTime { get; set; }


        public decimal Price { get; set; }


        public PackageEnum? Type { get; set; }

        public Student? StudentReservation { get; set; }


        public ICollection<Product> SelectedProducts { get; set; } = new List<Product>();


        public List<int> SelectedProductIds { get; set; } = new List<int> {};

        public CanteenEnum Canteen { get; set; }

        public CityEnum City { get; set; }

        public CanteenEnum MyCanteen { get; set; }

        public bool AgeRestriction { get; set; }
    }
}
