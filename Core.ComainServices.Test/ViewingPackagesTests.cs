using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests
{
    public class ViewingPackagesTests
    {
        [Fact]
        public void AvailablePackages_Dont_Have_StudentReservation()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var packages = new List<Package>
            {
                new Package
                {
                    Id = 1,
                    PickUp = DateTime.Now.AddDays(1),
                    StudentReservation = new Student(),
                },
                new Package
                {
                    Id = 2,
                    PickUp = DateTime.Now.AddDays(1),
                    StudentReservation = null,
                },
            };

            _packageRepositoryMock.Setup(r => r.GetAvailablePackages()).Returns(packages.Where(p => p.StudentReservation == null).ToList());

            // Act
            var availablePackages = sut.GetAvailablePackages();

            // Assert
            Assert.NotNull(availablePackages);
            Assert.Single(availablePackages);
            Assert.Equal(2, availablePackages.First().Id);
        }

        [Fact]
        public void ReservedPackages_Have_Pickup_DateTime()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var student = new Student()
            {
                StudentID = "1"
            };

            var packages = new List<Package>
            {
                new Package
                {
                    Id = 1,
                    PickUp = DateTime.Now.AddDays(1),
                    StudentReservation = student,
                },
                new Package
                {
                    Id = 2,
                    PickUp = DateTime.Now.AddDays(2),
                    StudentReservation = student,
                },
                new Package
                {
                    Id = 3,
                    PickUp = DateTime.Now.AddDays(2),
                    StudentReservation = null
                }
            };

            _packageRepositoryMock.Setup(r => r.GetAllReservationsFromStudent(student)).Returns(packages
                .Where(p => p.StudentReservation == student)
                .Where(p => p.PickUp >= DateTime.Today)
                .OrderBy(m => m.PickUp).ToList());

            // Act
            var reservedPackages = sut.GetAllReservationsFromStudent(student);

            // Assert
            Assert.NotNull(reservedPackages); 
            foreach (var package in reservedPackages)
            {
                Assert.NotEqual(DateTime.MinValue, package.PickUp);
            }
            Assert.Equal(2, reservedPackages.ToList().Count); 
            Assert.Contains(reservedPackages, p => p.Id == 1);
            Assert.Contains(reservedPackages, p => p.Id == 2);
        }

        [Fact]
        public void MyCanteenPackages_Are_From_MyCanteen_And_Are_Sorted_By_Date()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LA
            };

            var packages = new List<Package>
            {
                new Package
                {
                    Id = 1,
                    PickUp = DateTime.Now.AddDays(2),
                    Canteen = CanteenEnum.LA,
                },
                new Package
                {
                    Id = 2,
                    PickUp = DateTime.Now.AddDays(0),
                    Canteen = CanteenEnum.LA,
                },
                new Package
                {
                    Id = 3,
                    PickUp = DateTime.Now.AddDays(1),
                    Canteen = CanteenEnum.LD,
                },
                new Package
                {
                    Id = 4,
                    PickUp = DateTime.Now.AddDays(1),
                    Canteen = CanteenEnum.LA,
                }
            };

            _packageRepositoryMock.Setup(r => r.GetAllPackages()).Returns(packages
                .Where(p => p.Canteen == employee.WorkPlace)
                .Where(p => p.PickUp >= DateTime.Today)
                .OrderBy(p => p.PickUp).ToList());

            // Act
            var myCanteenPackages = sut.GetPackages();

            // Assert
            Assert.NotNull(myCanteenPackages);
            foreach (var package in myCanteenPackages)
            {
                Assert.NotEqual(DateTime.MinValue, package.PickUp);
            }
            Assert.Equal(3, myCanteenPackages.ToList().Count);
            Assert.DoesNotContain(myCanteenPackages, p => p.Id == 3);
            Assert.Equal(2, myCanteenPackages.ElementAt(0).Id); 
            Assert.Equal(4, myCanteenPackages.ElementAt(1).Id); 
            Assert.Equal(1, myCanteenPackages.ElementAt(2).Id);
        }

        [Fact]
        public void AllPackages_Are_From_AllCanteens_And_Are_Sorted_By_Date()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var packages = new List<Package>
            {
                new Package
                {
                    Id = 1,
                    PickUp = DateTime.Now.AddDays(2),
                    Canteen = CanteenEnum.LA,
                },
                new Package
                {
                    Id = 2,
                    PickUp = DateTime.Now.AddDays(0),
                    Canteen = CanteenEnum.LA,
                },
                new Package
                {
                    Id = 3,
                    PickUp = DateTime.Now.AddDays(1).AddHours(1),
                    Canteen = CanteenEnum.LD,
                },
                new Package
                {
                    Id = 4,
                    PickUp = DateTime.Now.AddDays(1),
                    Canteen = CanteenEnum.LA,
                }
            };

            _packageRepositoryMock.Setup(r => r.GetAllPackages()).Returns(packages
                .Where(p => p.PickUp >= DateTime.Today)
                .OrderBy(p => p.PickUp).ToList());

            // Act
            var myCanteenPackages = sut.GetPackages();

            // Assert
            Assert.NotNull(myCanteenPackages);
            foreach (var package in myCanteenPackages)
            {
                Assert.NotEqual(DateTime.MinValue, package.PickUp);
            }
            Assert.Equal(4, myCanteenPackages.ToList().Count);
            Assert.Equal(2, myCanteenPackages.ElementAt(0).Id);
            Assert.Equal(4, myCanteenPackages.ElementAt(1).Id);
            Assert.Equal(3, myCanteenPackages.ElementAt(2).Id);
            Assert.Equal(1, myCanteenPackages.ElementAt(3).Id);
        }

    }
}