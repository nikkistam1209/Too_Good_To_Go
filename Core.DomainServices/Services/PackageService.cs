using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Core.DomainServices.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Services
{
    public class PackageService : IPackageService
    {
        private readonly ILogger<PackageService> _logger;
        private readonly IPackageRepository _packageRepository;
        private readonly IStudentService _studentService;

        public PackageService(ILogger<PackageService> logger, IPackageRepository packageRepository, IStudentService studentService)
        {
            _logger = logger;
            _packageRepository = packageRepository;
            _studentService = studentService;
        }

        public async Task AddPackage(Package package)
        {
            try
            {
                await _packageRepository.AddPackage(package);
            }
            catch
            {
                throw new Exception("Package could not be added");
            }
        }

        public async Task UpdatePackage(Package package)
        {
            try
            {
                await _packageRepository.UpdatePackage(package);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating package: " + ex.Message);
            }
        }

        public async Task DeletePackage(Package package)
        {
            try
            {
                await _packageRepository.DeletePackage(package);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating package: " + ex.Message);
            }
        }


        public Package GetPackageById(int id)
        {
            try
            {
                return _packageRepository.GetPackageById(id);
            }
            catch
            {
                throw new Exception("Package could not be found");
            }
        }

        public IEnumerable<Package> GetAvailablePackages()
        {
            try
            {
                return _packageRepository.GetAvailablePackages();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting Packages: " + ex.Message);
            }
        }

        public IEnumerable<Package> GetPackages()
        {
            try
            {
                return _packageRepository.GetAllPackages();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting Packages: " + ex.Message);
            }
        }

        /*public IEnumerable<Package> GetMyCanteenPackages(CanteenEnum c)
        {
            try
            {
                return _packageRepository.GetMyCanteenPackages(c);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting Packages: " + ex.Message);
            }
        }

        public IEnumerable<Package> GetOtherCanteenPackages(CanteenEnum c)
        {
            try
            {
                return _packageRepository.GetOtherCanteenPackages(c);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting Packages: " + ex.Message);
            }
        }*/

        public async Task ReservePackageAsync(int PackageId, string userId)
        {
            var Package = GetPackageById(PackageId);

            if (Package != null && Package.StudentReservation == null)
            {
                var student = _studentService.GetStudentById(userId);

                if (student != null)
                {
                    // Check if the Package has an age restriction
                    if (Package.AgeRestriction)
                    {
                        var studentAge = CalculateStudentAge(student.DOB, Package.PickUp);

                        if (studentAge < 18) 
                        {
                            throw new Exception("You are not old enough to reserve this package");
                        }
                    }

                    // Check if the student already has a reservation on this day
                    var reservations = GetAllReservationsFromStudent(userId);
                    bool hasReservation = reservations.Any(r => r.PickUp == Package.PickUp);

                    if (hasReservation)
                    {
                        throw new Exception("You already have a reservation on this day");
                    }
                    else
                    {
                        Package.StudentReservation = student;
                        await _packageRepository.UpdatePackage(Package);
                    }
                }
                else
                {
                    throw new Exception("Student could not be found");
                }
            }
            else
            {
                throw new Exception("Package could not be found or already has a reservation");
            }
        }

        private int CalculateStudentAge(DateTime? dateOfBirth, DateTime pickUpDate)
        {
            if (dateOfBirth.HasValue)
            {
                int age = pickUpDate.Year - dateOfBirth.Value.Year;
                if (pickUpDate < dateOfBirth.Value.AddYears(age))
                {
                    age--;
                }
                return age;
            }
            else
            {
                throw new Exception("Date of birth is missing for the student");
            }
        }




        public IEnumerable<Package> GetAllReservationsFromStudent(string studentID)
        {
            var student = _studentService.GetStudentById(studentID);
            return _packageRepository.GetAllPackages().Where(m => m.StudentReservation == student).OrderBy(m => m.PickUp);
        }


    }
}
