using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Models;

namespace Portal.Controllers
{
    public class PackageController : Controller
    {
        private readonly ILogger<PackageController> _logger;
        private readonly IPackageService _packageService;
        private readonly IProductService _productService;
        private readonly IEmployeeService _employeeService;
        private readonly ICanteenService _canteenService;
        private readonly IStudentService _studentService;

        public PackageController(ILogger<PackageController> logger,
            IPackageService packageService,
            IProductService productService,
            IEmployeeService employeeService,
            ICanteenService canteenService,
            IStudentService studentService)
        {
            _logger = logger;
            _packageService = packageService;
            _productService = productService;
            _employeeService = employeeService;
            _canteenService = canteenService;
            _studentService = studentService;
        }

        // ------------------------------------------------ viewing packages ----------------------------------------------------

        [Authorize]
        public IActionResult AvailablePackages()
        {
            IEnumerable<Package> packages;
            CanteenEnum canteen;
            var viewModel = new AllPackagesModel { };

            // Getting the cities for the canteens
            var canteenToCityMapping = new Dictionary<CanteenEnum, CityEnum?>();
            foreach (var canteenEnum in Enum.GetValues(typeof(CanteenEnum)).Cast<CanteenEnum>())
            {
                var cityEnum = _canteenService.GetCityEnum(canteenEnum);
                if (cityEnum.HasValue)
                {
                    canteenToCityMapping[canteenEnum] = cityEnum.Value;
                }
            }

            if (User.HasClaim(c => c.Type == "Role" && c.Value == "Student"))
            {
                packages = _packageService.GetAvailablePackages();

                viewModel = new AllPackagesModel
                {
                    Packages = packages,
                    CanteenToCityMapping = canteenToCityMapping
                };
            }
            else
            {
                canteen = _employeeService.GetCanteenById(this.User.Identity?.Name);

                packages = _packageService.GetPackages();

                viewModel = new AllPackagesModel
                {
                    Packages = packages,
                    MyCanteen = canteen,
                    CanteenToCityMapping = canteenToCityMapping
                };

            }

            return View("AvailablePackages", viewModel);
        }

        [Authorize(Policy = "Employee")]
        public IActionResult MyCanteen()
        {
            CanteenEnum canteen = _employeeService.GetCanteenById(this.User.Identity?.Name);

            IEnumerable<Package> packages = _packageService.GetPackages();
            Dictionary<CanteenEnum, CityEnum?> canteenToCityMapping = NewMethod();

            var viewModel = new AllPackagesModel
            {
                Packages = packages,
                MyCanteen = canteen,
                CanteenToCityMapping = canteenToCityMapping
            };

            return View("MyCanteen", viewModel);

            Dictionary<CanteenEnum, CityEnum?> NewMethod()
            {
                // Getting the cities for the canteens
                var canteenToCityMapping = new Dictionary<CanteenEnum, CityEnum?>();
                foreach (var canteenEnum in Enum.GetValues(typeof(CanteenEnum)).Cast<CanteenEnum>())
                {
                    var cityEnum = _canteenService.GetCityEnum(canteenEnum);
                    if (cityEnum.HasValue)
                    {
                        canteenToCityMapping[canteenEnum] = cityEnum.Value;
                    }
                }

                return canteenToCityMapping;
            }
        }




        [Authorize]
        public IActionResult PackageDetails(int id)
        {
            try
            {
                var package = _packageService.GetPackageById(id);

                if (package != null && package.Canteen != null)
                {

                    var cityEnum = _canteenService.GetCityEnum(package.Canteen.Value);

                    if (cityEnum == null)
                    {
                        return NotFound();
                    }

                    var model = new PackageDetailModel
                    {
                        Id = package.Id,
                        Name = package.Name,
                        PickUpDate = package.PickUp,
                        ClosingTime = package.ClosingTime,
                        Price = package.Price,
                        Type = package.Type,
                        Canteen = package.Canteen.Value,
                        City = cityEnum.Value,
                        AgeRestriction = package.AgeRestriction,
                        StudentReservation = package.StudentReservation,
                        SelectedProducts = package.Products
                    };

                    if (User.HasClaim(c => c.Type == "Role" && c.Value == "Employee"))
                    {
                        model.MyCanteen = _employeeService.GetCanteenById(this.User.Identity?.Name);
                    }
                    return View(model);
                }

                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("AvailablePackages");
            }
        }

        // ------------------------------------------------------- reservations -----------------------------------------------

        [Authorize(Policy = "Student")]
        public IActionResult MyReservations()
        {
            var studentId = this.User.Identity?.Name;
            var packages = _packageService.GetAllReservationsFromStudent(studentId);

            // Getting the cities for the canteens
            var canteenToCityMapping = new Dictionary<CanteenEnum, CityEnum?>();
            foreach (var canteenEnum in Enum.GetValues(typeof(CanteenEnum)).Cast<CanteenEnum>())
            {
                var cityEnum = _canteenService.GetCityEnum(canteenEnum);
                if (cityEnum.HasValue)
                {
                    canteenToCityMapping[canteenEnum] = cityEnum.Value;
                }
            }

            var viewModel = new AllPackagesModel
            {
                Packages = packages,
                CanteenToCityMapping = canteenToCityMapping
            };

            return View(viewModel);
        }



        [Authorize(Policy = "Student")]
        public async Task<IActionResult> ReservePackage(int id)
        {
            var studentId = this.User.Identity?.Name;
            var package = _packageService.GetPackageById(id);

            if (package == null)
            {
                TempData["ErrorMessage"] = "Package could not be found";
                return RedirectToAction("AvailablePackages");
            }

            Dictionary<CanteenEnum, CityEnum?> canteenToCityMapping = CanteenCity();

            var viewModel = new AllPackagesModel
            {
                Packages = _packageService.GetAvailablePackages(),
                CanteenToCityMapping = canteenToCityMapping
            };

            try
            {
                await _packageService.ReservePackageAsync(id, studentId);
                TempData["SuccessMessage"] = "Package reserved successfully";
                return RedirectToAction("MyReservations");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Reservation failed: " + ex.Message;
                viewModel.PackageError = ex.Message;
            }

            return View("AvailablePackages", viewModel);

        }

        private Dictionary<CanteenEnum, CityEnum?> CanteenCity()
        {
            // Getting the cities for the canteens
            var canteenToCityMapping = new Dictionary<CanteenEnum, CityEnum?>();
            foreach (var canteenEnum in Enum.GetValues(typeof(CanteenEnum)).Cast<CanteenEnum>())
            {
                var cityEnum = _canteenService.GetCityEnum(canteenEnum);
                if (cityEnum.HasValue)
                {
                    canteenToCityMapping[canteenEnum] = cityEnum.Value;
                }
            }

            return canteenToCityMapping;
        }


        // ----------------------------------------------------- creating a package -----------------------------------------------

        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> CreatePackage()
        {
            try
            {
                var availableProducts = _productService.GetAllProducts();

                var availableProductsList = availableProducts
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToList();

                var canteenLocation = _employeeService.GetCanteenById(this.User.Identity!.Name!);

                var myCanteen = await _canteenService.GetCanteenByLocationAsync(canteenLocation);

                if (myCanteen == null)
                {
                    TempData["ErrorMessage"] = "Your canteen could not be retrieved";
                    return View();
                }

                var model = new PackageModel
                {
                    AvailableProducts = availableProductsList,
                    PickUpDate = DateTime.Now.Date,
                    MyCanteenOffersDinners = myCanteen.OffersDinners
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " + ex.Message;
                return View();
            }
        }



        [HttpPost]
        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> CreatePackage(PackageModel packageModel)
        {
            if (!ModelState.IsValid)
            {
                var availableProducts = _productService.GetAllProducts();

                packageModel.AvailableProducts = availableProducts.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                return View(packageModel);
            }

            try
            {
                var employee = _employeeService.GetEmployeeById(this.User.Identity?.Name);

                var pickUpDateTime = packageModel.PickUpDate + packageModel.PickUpTime;
                var closingTimeDateTime = packageModel.PickUpDate + packageModel.ClosingTime;


                var package = new Package()
                {
                    Name = packageModel.Name,
                    Canteen = employee.WorkPlace,
                    PickUp = pickUpDateTime,
                    ClosingTime = closingTimeDateTime,
                    Price = packageModel.Price,
                    Type = packageModel.Type

                };


                var selectedProducts = _productService.GetProductsByIds(packageModel.SelectedProductIds).ToList();

                bool containsAlcohol = selectedProducts.Any(p => p.ContainsAlcohol);

                package.AgeRestriction = containsAlcohol;

                package.Products = selectedProducts;


               
                await _packageService.AddPackage(package);

                TempData["SuccessMessage"] = "Package created successfully";
                return RedirectToAction("AvailablePackages");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " + ex.Message;
                return View(packageModel);
            }

        }

        // ---------------------------------------------------- updating a package -------------------------------------------------

        [Authorize(Policy = "Employee")]
        public IActionResult EditPackage(int id)
        {
            var package = _packageService.GetPackageById(id);

            if (package == null)
            {
                // Handle the case where the package with the given id does not exist
                return RedirectToAction("AvailablePackages");
            }

            var availableProducts = _productService.GetAllProducts();
            var availableProductsList = availableProducts.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            var selectedProductIds = package.Products.Select(p => p.Id).ToList();

            var model = new PackageModel
            {
                Id = package.Id,
                Name = package.Name,
                AvailableProducts = availableProductsList,
                SelectedProductIds = selectedProductIds,
                PickUpDate = package.PickUp.Date,
                PickUpTime = package.PickUp.TimeOfDay,
                ClosingTime = package.ClosingTime.TimeOfDay,
                Price = package.Price,
                Type = package.Type
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> EditPackage(PackageModel packageModel)
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors
                var availableProducts = _productService.GetAllProducts();
                packageModel.AvailableProducts = availableProducts.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
                return View(packageModel);
            }

            try
            {
                var existingPackage = _packageService.GetPackageById(packageModel.Id);

                if (existingPackage == null)
                {
                    return RedirectToAction("AvailablePackages");
                }
                var pickUpDateTime = packageModel.PickUpDate + packageModel.PickUpTime;
                var closingTimeDateTime = packageModel.PickUpDate + packageModel.ClosingTime;

                var selectedProducts = _productService.GetProductsByIds(packageModel.SelectedProductIds).ToList();

                bool containsAlcohol = selectedProducts.Any(p => p.ContainsAlcohol);

                // Update the properties of the existing package with the values from the model
                existingPackage.Name = packageModel.Name;
                existingPackage.PickUp = pickUpDateTime;
                existingPackage.ClosingTime = closingTimeDateTime;
                existingPackage.Price = packageModel.Price;
                existingPackage.Type = packageModel.Type;
                existingPackage.AgeRestriction = containsAlcohol;
                existingPackage.Products = selectedProducts;

                await _packageService.UpdatePackage(existingPackage);
                TempData["SuccessMessage"] = "Package edited successfully";
                return RedirectToAction("AvailablePackages");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " + ex.Message;
                return View(packageModel);
            }
        }

        // ---------------------------------------------------- deleting a package -------------------------------------------------

        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = _packageService.GetPackageById(id);

            if (package == null)
            {
                TempData["ErrorMessage"] = "The package could not be found";
                return RedirectToAction("AvailablePackages");
            }

            try
            {
                if (package.StudentReservation != null)
                {
                    TempData["ErrorMessage"] = "This package has a reservation and cannot be deleted";
                }
                else
                {
                    await _packageService.DeletePackage(package);
                    TempData["SuccessMessage"] = "Package deleted successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " +  ex.Message;
            }

            return RedirectToAction("AvailablePackages");
        }





    }
}
