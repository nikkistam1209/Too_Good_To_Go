using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests
{
    public class CreatingEditingDeletingPackagesTests
    {
        // ------------------------------------------------------ creating packages ------------------------------------------------------

        [Fact]
        public async Task Create_Package_With_Employee_Canteen()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var products = new List<Product>();

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LA
            };

            var package = new Package{};

            // Act
            await sut.AddPackage(package, employee.WorkPlace, products);

            // Assert
            _packageRepositoryMock.Verify(repo => repo.AddPackage(It.Is<Package>(p => p.Canteen == employee.WorkPlace)), Times.Once);
        }

        [Fact]
        public async Task Add_Package_With_Alcohol_Restriction_And_Product_Containing_Alcohol()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var products = new List<Product>()
            {
                new Product()
                {
                    ContainsAlcohol = true,
                }
            };

            var package = new Package{};

            // Act
            await sut.AddPackage(package, CanteenEnum.LA, products);

            // Assert
            _packageRepositoryMock.Verify(repo => repo.AddPackage(It.Is<Package>(p => p.AgeRestriction == true)), Times.Once);
        }

        // ------------------------------------------------------ editing packages ------------------------------------------------------

        [Fact]
        public async Task Can_Edit_Package_No_Reservation_And_Your_Canteen()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var products = new List<Product>();

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LA
            };

            var package = new Package
            {
                Canteen = CanteenEnum.LA
            };

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.UpdatePackage(package, employee.WorkPlace, products));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Can_Not_Edit_Package_Not_Your_Canteen()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var products = new List<Product>();

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LD
            };

            var student = new Student(){};

            var package = new Package
            {
                Canteen = CanteenEnum.LA,
                StudentReservation = student
            };

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.UpdatePackage(package, employee.WorkPlace, products));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("You do not have permission to update this package", result.Message);
        }

        [Fact]
        public async Task Can_Not_Edit_Package_With_Reservation()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var products = new List<Product>();

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LA
            };

            var student = new Student(){};

            var package = new Package
            {
                Canteen = CanteenEnum.LA,
                StudentReservation = student
            };

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.UpdatePackage(package, employee.WorkPlace, products));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("This package has a reservation and cannot be updated", result.Message);
        }

        // ------------------------------------------------------ deleting packages ------------------------------------------------------

        [Fact]
        public async Task Can_Delete_Package_No_Reservation_And_Your_Canteen()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LA
            };

            var package = new Package
            {
                Canteen = CanteenEnum.LA
            };

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.DeletePackage(package, employee.WorkPlace));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Can_Not_Delete_Package_Not_Your_Canteen()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LD
            };

            var student = new Student(){};

            var package = new Package
            {
                Canteen = CanteenEnum.LA
            };

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.DeletePackage(package, employee.WorkPlace));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("You do not have permission to delete this package", result.Message);
        }

        [Fact]
        public async Task Can_Not_Delete_Package_With_Reservation()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var employee = new Employee()
            {
                WorkPlace = CanteenEnum.LA
            };

            var student = new Student(){};

            var package = new Package
            {
                Canteen = CanteenEnum.LA,
                StudentReservation = student
            };

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.DeletePackage(package, employee.WorkPlace));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("This package has a reservation and cannot be deleted", result.Message);
        }


    }
}