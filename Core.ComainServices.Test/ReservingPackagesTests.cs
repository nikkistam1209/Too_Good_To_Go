using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IRepositories;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests
{
    public class ReservingPackagesTests
    {
        [Fact]
        public async Task Can_Reserve_Package_With_No_Reservation()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var package = new Package()
            {
                Id = 1,
                StudentReservation = null,
                PickUp = DateTime.Now.AddDays(2),
                AgeRestriction = true
            };

            var student = new Student()
            {
                StudentID = "1",
                DOB = DateTime.Now.AddYears(-20)
            };

            _packageRepositoryMock.Setup(repo => repo.GetPackageById(package.Id)).Returns(package);
            _studentServiceMock.Setup(service => service.GetStudentById(student.StudentID)).Returns(student);

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.ReservePackageAsync(package.Id, student.StudentID));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Can_Not_Reserve_Package_If_Not_Old_Enough_And_Contains_Alcohol()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var package = new Package()
            {
                Id = 1,
                AgeRestriction = true,
                PickUp = DateTime.Now.AddDays(2),
                StudentReservation = null
            };

            var student = new Student()
            {
                StudentID = "1",
                DOB = DateTime.Now.AddYears(-16)
            };

            _packageRepositoryMock.Setup(repo => repo.GetPackageById(package.Id)).Returns(package);
            _studentServiceMock.Setup(service => service.GetStudentById(student.StudentID)).Returns(student);

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.ReservePackageAsync(package.Id, student.StudentID));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("You are not old enough to reserve this package", result.Message); 
        }

        [Fact]
        public async Task Can_Not_Reserve_Package_With_Reservation()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var package = new Package()
            {
                Id = 1,
                StudentReservation = new Student(),
                PickUp = DateTime.Now.AddDays(2)
            };

            var student = new Student()
            {
                StudentID = "1"
            };

            _packageRepositoryMock.Setup(repo => repo.GetPackageById(package.Id)).Returns(package);
            _studentServiceMock.Setup(service => service.GetStudentById(student.StudentID)).Returns(student);

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.ReservePackageAsync(package.Id, student.StudentID));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Package is already reserved", result.Message);
        }


        [Fact]
        public async Task Can_Not_Reserve_Package_If_I_Have_Reservation_On_Same_Day()
        {
            // Arrange
            var _packageRepositoryMock = new Mock<IPackageRepository>();
            var _studentServiceMock = new Mock<IStudentService>();

            var sut = new PackageService(_packageRepositoryMock.Object, _studentServiceMock.Object);

            var package = new Package()
            {
                Id = 1,
                StudentReservation = null,
                PickUp = DateTime.Now.AddDays(2)
            };

            var student = new Student()
            {
                StudentID = "1"
            };

            var studentReservations = new List<Package>
            {
                new Package
                {
                    Id = 2,
                    PickUp = DateTime.Now.AddDays(2),
                    StudentReservation = student
                },
            };

            _packageRepositoryMock.Setup(repo => repo.GetAllReservationsFromStudent(student)).Returns(studentReservations);
            _packageRepositoryMock.Setup(repo => repo.GetPackageById(package.Id)).Returns(package);
            _studentServiceMock.Setup(service => service.GetStudentById(student.StudentID)).Returns(student);

            // Act
            Exception result = await Record.ExceptionAsync(() => sut.ReservePackageAsync(package.Id, student.StudentID));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("You already have a reservation on this day", result.Message);

        } 

    }
}