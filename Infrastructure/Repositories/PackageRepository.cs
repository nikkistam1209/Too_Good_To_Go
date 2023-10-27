using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly ApplicationDbContext _context;

        public PackageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Package> GetAvailablePackages()
        {
            return _context.Packages
                .Where(p => p.PickUp >= DateTime.Today)
                .Include(p => p.Products)
                .Where(p => p.StudentReservation == null)
                .OrderBy(p => p.PickUp)
                .ToList();
        }

        public IEnumerable<Package> GetAllPackages()
        {
            return _context.Packages
                .Where(p => p.PickUp >= DateTime.Today)
                .Include(p => p.Products)
                .Include(p => p.StudentReservation)
                .OrderBy(p => p.PickUp)
                .ToList();
        }

        public Package GetPackageById(int id)
        {
            return _context.Packages
                .Include(p => p.Products)
                .Include(p => p.StudentReservation)              
                .First(s => s.Id == id);
        }

        public IEnumerable<Package> GetAllReservationsFromStudent(Student student)
        {
            return _context.Packages
                .Where(m => m.StudentReservation == student)
                .Where(p => p.PickUp >= DateTime.Today)
                .Include(p => p.Products)
                .OrderBy(m => m.PickUp);
        }

        public async Task AddPackage(Package newPackage)
        {
            _context.Packages.Add(newPackage);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePackage(Package package)
        {
            _context.Update(package);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePackage(Package package)
        {
            _context.Remove(package);
            await _context.SaveChangesAsync();
        }


    }
}
