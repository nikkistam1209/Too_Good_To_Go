using Core.Domain.Entities;
using Core.Domain.Enumerations;

namespace Core.DomainServices.IServices
{
    public interface IPackageService
    {
        Package GetPackageById(int id);

        Task AddPackage(Package package, CanteenEnum workPlace, List<Product> selectedProducts);
        Task UpdatePackage(Package package, CanteenEnum canteenEnum, List<Product> selectedProducts);
        Task DeletePackage(Package package, CanteenEnum canteenEnum);


        IEnumerable<Package> GetAvailablePackages();
        IEnumerable<Package> GetPackages();

        Task ReservePackageAsync(int packageId, string userId);

        IEnumerable<Package> GetAllReservationsFromStudent(Student student);
    }
}