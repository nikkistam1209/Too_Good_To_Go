using Core.Domain.Entities;
using Core.Domain.Enumerations;

namespace Core.DomainServices.IServices
{
    public interface IPackageService
    {
        Package GetPackageById(int id);

        Task AddPackage(Package package);

        Task UpdatePackage(Package package);
        IEnumerable<Package> GetAvailablePackages();
        IEnumerable<Package> GetPackages();

        IEnumerable<Package> GetMyCanteenPackages(CanteenEnum c);
        IEnumerable<Package> GetOtherCanteenPackages(CanteenEnum c);

        Task ReservePackageAsync(int packageId, string userId);

        IEnumerable<Package> GetAllReservationsFromStudent(string studentID);
    }
}