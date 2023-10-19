using Core.Domain.Entities;
using Core.Domain.Enumerations;

namespace Portal.Models
{
    public class AllPackagesModel
    {
        public IEnumerable<Package> Packages { get; set; }

        public CanteenEnum? MyCanteen { get; set; }

        public string ReservePackageError { get; set; }

        public bool oldEnough { get; set; }
    }
}
