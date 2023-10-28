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
        private readonly IPackageRepository _packageRepository;
        private readonly IStudentService _studentService;

        public PackageService(IPackageRepository packageRepository, IStudentService studentService)
        {
            _packageRepository = packageRepository;
            _studentService = studentService;
        }

        public async Task AddPackage(Package package, CanteenEnum employeeCanteen, List<Product> selectedProducts)
        {
            try
            {
                package.Canteen = employeeCanteen;
                package.Products = selectedProducts;
                package.AgeRestriction = selectedProducts.Any(p => p.ContainsAlcohol);
                await _packageRepository.AddPackage(package);
            }
            catch
            {
                throw new Exception("Package could not be added");
            }
        }

        public async Task UpdatePackage(Package package, CanteenEnum employeeCanteen, List<Product> selectedProducts)
        {
            try
            {
                if (employeeCanteen != package.Canteen)
                {
                    throw new Exception("You do not have permission to update this package");
                }
                if (package.StudentReservation != null)
                {
                    throw new Exception("This package has a reservation and cannot be updated");
                }
                package.Products = selectedProducts;
                package.AgeRestriction = selectedProducts.Any(p => p.ContainsAlcohol);
                await _packageRepository.UpdatePackage(package);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeletePackage(Package package, CanteenEnum employeeCanteen)
        {
            try
            {
                if (employeeCanteen != package.Canteen)
                {
                    throw new Exception("You do not have permission to delete this package");
                }
                if (package.StudentReservation != null)
                {
                    throw new Exception("This package has a reservation and cannot be deleted");
                }
                await _packageRepository.DeletePackage(package);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

        public IEnumerable<Package> GetAllReservationsFromStudent(Student student)
        {
            try
            {
                return _packageRepository.GetAllReservationsFromStudent(student);
            }
            catch
            {
                throw new Exception("Reservations could not be found");
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

        public async Task ReservePackageAsync(int PackageId, string userId)
        {
            var Package = GetPackageById(PackageId);

            if (Package != null)
            {
                if (Package.StudentReservation == null)
                {
                    var student = _studentService.GetStudentById(userId);

                    if (student != null && student.StudentID != null)
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
                        var reservations = GetAllReservationsFromStudent(student);

                        if (reservations != null)
                        {
                            bool hasReservation = reservations.Any(r => r.PickUp.Date == Package.PickUp.Date);

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
                    }
                    else
                    {
                        throw new Exception("Student could not be found");
                    }
                }
                else
                {
                    throw new Exception("Package is already reserved");
                }
            }
            else
            {
                throw new Exception("Package could not be found");
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




       


    }
}
