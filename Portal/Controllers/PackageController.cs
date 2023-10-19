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

        // ------------------------- view packages --------------------------

        [Authorize]
        public IActionResult AvailablePackages(string selectedOption = "all")
        {
            IEnumerable<Package> packages;
            CanteenEnum canteen;
            var viewModel = new AllPackagesModel { };

            if (User.HasClaim(c => c.Type == "Role" && c.Value == "Student"))
            {
                //canteen = _studentService.GetCanteenById(this.User.Identity?.Name);

                packages = _packageService.GetAvailablePackages();

                viewModel = new AllPackagesModel
                {
                    Packages = packages
                };
            }
            else
            {
                canteen = _employeeService.GetCanteenById(this.User.Identity?.Name);

                packages = _packageService.GetPackages();

                viewModel = new AllPackagesModel
                {
                    Packages = packages,
                    MyCanteen = canteen
                };

            }

            

            return View("AvailablePackages", viewModel);
        }

        [Authorize(Policy = "Employee")]
        public IActionResult MyCanteen()
        {
            CanteenEnum canteen = _employeeService.GetCanteenById(this.User.Identity?.Name);

            IEnumerable<Package> packages = _packageService.GetPackages();

            var viewModel = new AllPackagesModel
            {
                Packages = packages,
                MyCanteen = canteen
            };

            return View("MyCanteen", viewModel);
        }


        /*[Authorize]
        public IActionResult AvailablePackages(string selectedOption = "all")
        {
            IEnumerable<Package> packages;

            _logger.LogInformation("-----------" + User.HasClaim(c => c.Type == "Role" && c.Value == "Student"));

            if (User.HasClaim(c => c.Type == "Role" && c.Value == "Student"))
            {
                packages = _packageService.GetAvailablePackages();
            }
            else
            {
                CanteenEnum canteen = _employeeService.GetCanteenById(this.User.Identity?.Name);

                if (selectedOption == "my")
                {
                    packages = _packageService.GetMyCanteenPackages(canteen);
                }
                else if (selectedOption == "other")
                {
                    packages = _packageService.GetOtherCanteenPackages(canteen);
                }
                else // if selected option = all
                {
                    packages = _packageService.GetPackages();
                }
            }

            var viewModel = new AllPackagesModel
            {
                Packages = packages
            };

            return View("AvailablePackages", viewModel);
        }*/


        /*[Authorize]
        public IActionResult MyPackages(string selectedOption = "all")
        {
            IEnumerable<Package> packages;

            _logger.LogInformation("-----------" + User.HasClaim(c => c.Type == "Role" && c.Value == "Student"));

            if (User.HasClaim(c => c.Type == "Role" && c.Value == "Student"))
            {
                // filter op kantines in mijn stad
                packages = _packageService.GetAvailablePackages();
            }
            else
            {
                CanteenEnum canteen = _employeeService.GetCanteenById(this.User.Identity?.Name);

                if (selectedOption == "my")
                {
                    packages = _packageService.GetMyCanteenPackages(canteen);
                }
                else if (selectedOption == "other")
                {
                    packages = _packageService.GetOtherCanteenPackages(canteen);
                }
                else // if selected option = all
                {
                    packages = _packageService.GetPackages();
                }
            }

            var viewModel = new AllPackagesModel
            {
                Packages = packages
            };

            return View("AvailablePackages", viewModel);
        }*/









        [Authorize]
        public IActionResult PackageDetails(int id)
        {
            _logger.LogInformation("package id: " + id);

            var package = _packageService.GetPackageById(id);
            if (package == null)
            {
                return NotFound(); // Handle the case where the package is not found.
            }

            var model = new PackageDetailModel
            {
                Id = package.Id,
                Name = package.Name,
                PickUpDate = package.PickUp,
                PickUpTime = package.PickUp.TimeOfDay,
                ClosingTime = package.ClosingTime.TimeOfDay,
                Price = package.Price,
                Type = package.Type,
                Canteen = package.Canteen.Value,
                AgeRestriction = package.AgeRestriction,
                StudentReservation = package.StudentReservation,
                SelectedProducts = package.Products
            };

            return View(model);
        }

        // ------------------------ reservations -------------------------

        [Authorize(Policy = "Student")]
        public IActionResult MyReservations()
        {
            var studentId = this.User.Identity?.Name;
            var packages = _packageService.GetAllReservationsFromStudent(studentId);

            return View(packages);
        }



        [Authorize(Policy = "Student")]
        public async Task<IActionResult> ReservePackage(int id)
        {
            var studentId = this.User.Identity?.Name;
            var package = _packageService.GetPackageById(id);

            if (package == null)
            {
                return NotFound();
            }

            var viewModel = new AllPackagesModel
            {
                Packages = _packageService.GetAvailablePackages(),
            };

            try
            {
                await _packageService.ReservePackageAsync(id, studentId);
                return RedirectToAction("MyReservations");
            }
            catch (Exception ex)
            {
                viewModel.ReservePackageError = ex.Message;
            }

            return View("AvailablePackages", viewModel);

        }


        // ------------------------ creating a package ------------------------

        [Authorize(Policy = "Employee")]
        public IActionResult CreatePackage()
        {
            var availableProducts = _productService.GetAllProducts();

            var availableProductsList = availableProducts.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            var model = new PackageModel
            {
                AvailableProducts = availableProductsList,
                PickUpDate = DateTime.Now.Date,
            };
            return View(model);
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

                var employeeId = this.User.Identity?.Name;
                var employee = _employeeService.GetEmployeeById(employeeId);

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

                //package.Products = (ICollection<Product>)_productService.GetProductsByIds(packageModel.SelectedProductIds);

                var selectedProducts = _productService.GetProductsByIds(packageModel.SelectedProductIds).ToList();

                bool containsAlcohol = selectedProducts.Any(p => p.ContainsAlcohol);

                package.AgeRestriction = containsAlcohol;

                package.Products = selectedProducts;



                await _packageService.AddPackage(package);

                return RedirectToAction("AvailablePackages");

            }
            catch (Exception ex)
            {
                return View(packageModel);
            }

        }

        // ------------------------ updating a package ------------------------

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

                return RedirectToAction("AvailablePackages");
            }
            catch (Exception ex)
            {
                return View(packageModel);
            }
        }



    }
}
