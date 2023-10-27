using Core.Domain.Entities;
using Core.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.IRepositories
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetAvailablePackages();
        IEnumerable<Package> GetAllPackages();

        IEnumerable<Package> GetAllReservationsFromStudent(Student student);

        Package GetPackageById(int id);

        Task AddPackage(Package newPackage);
        Task UpdatePackage(Package package);
        Task DeletePackage(Package package);
    }
}
