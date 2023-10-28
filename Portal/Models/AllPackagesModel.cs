using Core.Domain.Entities;
using Core.Domain.Enumerations;

namespace Portal.Models
{
    public class AllPackagesModel
    {
        public IEnumerable<Package>? Packages { get; set; }

        public CanteenEnum? MyCanteen { get; set; }

        public string? PackageError { get; set; }

        public bool oldEnough { get; set; }

        public Dictionary<CanteenEnum, CityEnum?>? CanteenToCityMapping { get; set; }
    }
}
