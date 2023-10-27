using System;
using Xunit;
using Core.Domain.Entities;
using Portal.Models;
using System.ComponentModel.DataAnnotations;
using Portal.Tests;
using Core.Domain.Enumerations;

namespace Core.Domain.Test
{
    public class PackageModelTests
    {
        [Fact]
        public void No_Name_Gives_Error()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                PickUpDate = DateTime.Today.AddDays(1),
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Price = 2,
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 }
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Contains("Please give the package a descriptive name", result);
        }

        [Fact]
        public void Valid_PickUpDate_Gives_No_Error()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Price = 2,
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };


            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Invalid_PickUpDate_Gives_Error()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(3),
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Price = 2,
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Contains("Please fill out a date between today and two days from now", result);
        }

        [Fact]
        public void Valid_PickUpTime_Gives_No_Error()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                Price = 2,
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void InValid_PickUpTime_Gives_Error()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                PickUpTime = TimeSpan.FromHours(5),
                ClosingTime = TimeSpan.FromHours(20),
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                Price = 2,
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Contains("Please choose a reasonable pickup time", result);
        }

        [Fact]
        public void Valid_Closing_Time()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                PickUpTime = TimeSpan.FromHours(12),
                ClosingTime = TimeSpan.FromHours(13),
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                Price = 2,
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void InValid_Closing_Time()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                PickUpTime = TimeSpan.FromHours(12),
                ClosingTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(30)),
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                Price = 2,
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Contains("Please choose a reasonable closing time that is at least one hour from pickup time", result);
        }

        [Fact]
        public void Valid_Price()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                Price = 2,
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void InValid_Price()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                Price = 300,
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Type = PackageEnum.Snack,
                SelectedProductIds = new List<int> { 1, 2, 3 },
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Contains("Please give the package a fitting price", result);
        }

        [Fact]
        public void No_Selected_Products_Error()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Price = 2,
                Type = PackageEnum.Snack
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Contains("Please select at least one product", result);
        }

        [Fact]
        public void Valid_Selected_Products()
        {
            // Arrange
            var validator = new ModelValidator();
            var package = new PackageModel
            {
                SelectedProductIds = new List<int> { 1, 2, 3 },
                Name = "Test",
                PickUpDate = DateTime.Today.AddDays(1),
                PickUpTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(20),
                Price = 2,
                Type = PackageEnum.Snack
            };

            // Act
            var result = validator.ValidateModel(package);

            // Assert
            Assert.Empty(result);
        }

        
    }
}
