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
        private readonly IPackageService _packageService;
        private readonly IProductService _productService;
        private readonly IEmployeeService _employeeService;
        private readonly ICanteenService _canteenService;
        private readonly IStudentService _studentService;

        public PackageController(
            IPackageService packageService,
            IProductService productService,
            IEmployeeService employeeService,
            ICanteenService canteenService,
            IStudentService studentService)
        {
            _packageService = packageService;
            _productService = productService;
            _employeeService = employeeService;
            _canteenService = canteenService;
            _studentService = studentService;
        }

        // ------------------------------------------------ viewing packages ----------------------------------------------------

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
                        model.MyCanteen = _employeeService.GetCanteenById(this.User.Identity!.Name!);
                    }
                    return View(model);
                }
                TempData["ErrorMessage"] = "Package could not be found";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;              
            }
            return RedirectToAction("AvailablePackages");
        }


        [Authorize]
        public IActionResult AvailablePackages()
        {
            try
            {
                IEnumerable<Package> packages;
                CanteenEnum canteen;
                var viewModel = new AllPackagesModel { };

                Dictionary<CanteenEnum, CityEnum?> canteenToCityMapping = CanteenCity();

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
                    canteen = _employeeService.GetCanteenById(this.User.Identity!.Name!);

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
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Home/Index");
        }     

        // ------------------------------------------------------- my canteen packages -----------------------------------------------

        [Authorize(Policy = "Employee")]
        public IActionResult MyCanteen()
        {
            try
            {
                var employeeId = this.User.Identity?.Name;
                if (employeeId != null)
                {
                    CanteenEnum canteen = _employeeService.GetCanteenById(employeeId);

                    IEnumerable<Package> packages = _packageService.GetPackages();
                    Dictionary<CanteenEnum, CityEnum?> canteenToCityMapping = CanteenCity();

                    var viewModel = new AllPackagesModel
                    {
                        Packages = packages,
                        MyCanteen = canteen,
                        CanteenToCityMapping = canteenToCityMapping
                    };

                    return View("MyCanteen", viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = "User is not authenticated or their identity can not be found";
                }
            } 
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("AvailablePackages");
        }

        // ------------------------------------------------------- my reservations and reserving packages -----------------------------------------------

        [Authorize(Policy = "Student")]
        public IActionResult MyReservations()
        {
            try
            {
                var studentId = this.User.Identity?.Name;
                if (studentId != null)
                {
                    Student student = _studentService.GetStudentById(studentId);
                    var packages = _packageService.GetAllReservationsFromStudent(student);

                    Dictionary<CanteenEnum, CityEnum?> canteenToCityMapping = CanteenCity();

                    var viewModel = new AllPackagesModel
                    {
                        Packages = packages,
                        CanteenToCityMapping = canteenToCityMapping
                    };
                    return View(viewModel);
                }
                else
                {
                    TempData["ErrorMessage"] = "User is not authenticated or their identity can not be found";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("AvailablePackages");
            
        }

        [Authorize(Policy = "Student")]
        public async Task<IActionResult> ReservePackage(int id)
        {
            try 
            { 
                var studentId = this.User.Identity?.Name;
                if (studentId != null)
                {
                    var package = _packageService.GetPackageById(id);

                    Dictionary<CanteenEnum, CityEnum?> canteenToCityMapping = CanteenCity();

                    var viewModel = new AllPackagesModel
                    {
                        Packages = _packageService.GetAvailablePackages(),
                        CanteenToCityMapping = canteenToCityMapping
                    };

                    await _packageService.ReservePackageAsync(id, studentId);
                    TempData["SuccessMessage"] = "Package reserved successfully";
                    return RedirectToAction("MyReservations");
                }
                else
                {
                    TempData["ErrorMessage"] = "User is not authenticated or their identity can not be found";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Reservation failed: " + ex.Message;              
            }
            return RedirectToAction("AvailablePackages");
        }

        

        // ----------------------------------------------------- creating a package -----------------------------------------------

        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> CreatePackage()
        {
            try
            {
                // get available products
                var availableProducts = _productService.GetAllProducts();
                var availableProductsList = availableProducts
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToList();

                // get employee canteen for offersdinners
                var canteenLocation = _employeeService.GetCanteenById(this.User.Identity!.Name!);
                var myCanteen = await _canteenService.GetCanteenByLocationAsync(canteenLocation);

                var model = new PackageModel
                {
                    AvailableProducts = availableProductsList,
                    PickUpDate = DateTime.Now.Date,
                    MyCanteenOffersDinners = myCanteen!.OffersDinners
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " + ex.Message;
                return RedirectToAction("AvailablePackages");
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
                var package = new Package()
                {
                    Name = packageModel.Name,
                    PickUp = packageModel.PickUpDate + packageModel.PickUpTime,
                    ClosingTime = packageModel.PickUpDate + packageModel.ClosingTime,
                    Price = packageModel.Price,
                    Type = packageModel.Type
                };

                var employee = _employeeService.GetEmployeeById(this.User.Identity!.Name!);
                var selectedProducts = _productService.GetProductsByIds(packageModel.SelectedProductIds).ToList();
                            
                await _packageService.AddPackage(package, employee.WorkPlace, selectedProducts);

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
        public async Task<IActionResult> EditPackage(int id)
        {
            try
            {
                var package = _packageService.GetPackageById(id);

                var availableProducts = _productService.GetAllProducts();
                var availableProductsList = availableProducts.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                var selectedProductIds = package.Products.Select(p => p.Id).ToList();

                // get employee canteen for offersdinners
                var canteenLocation = _employeeService.GetCanteenById(this.User.Identity!.Name!);
                var myCanteen = await _canteenService.GetCanteenByLocationAsync(canteenLocation);

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
                    Type = package.Type,
                    MyCanteenOffersDinners = myCanteen!.OffersDinners
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " + ex.Message;
                return RedirectToAction("AvailablePackages");
            }
        }

        [HttpPost]
        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> EditPackage(PackageModel packageModel)
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
                var updatePackage = _packageService.GetPackageById(packageModel.Id);
                updatePackage.Name = packageModel.Name;
                updatePackage.PickUp = packageModel.PickUpDate + packageModel.PickUpTime; ;
                updatePackage.ClosingTime = packageModel.PickUpDate + packageModel.ClosingTime; ;
                updatePackage.Price = packageModel.Price;
                updatePackage.Type = packageModel.Type;

                var employee = _employeeService.GetEmployeeById(this.User.Identity!.Name!);
                var selectedProducts = _productService.GetProductsByIds(packageModel.SelectedProductIds).ToList();

                await _packageService.UpdatePackage(updatePackage, employee.WorkPlace, selectedProducts);
                TempData["SuccessMessage"] = "Package edited successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " + ex.Message;              
            }
            return RedirectToAction("AvailablePackages");
        }

        // ---------------------------------------------------- deleting a package -------------------------------------------------

        [Authorize(Policy = "Employee")]
        public async Task<IActionResult> DeletePackage(int id)
        {         
            try
            {
                var canteenLocation = _employeeService.GetCanteenById(this.User.Identity!.Name!);
                var package = _packageService.GetPackageById(id);
                await _packageService.DeletePackage(package, canteenLocation);
                TempData["SuccessMessage"] = "Package deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred: " +  ex.Message;
            }
            return RedirectToAction("AvailablePackages");
        }

        // -------------------------------------------- dictionary for city to canteen mapping ------------------------------------------------
        private Dictionary<CanteenEnum, CityEnum?> CanteenCity()
        {
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
}
