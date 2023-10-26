﻿using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using System.Security.Claims;

namespace Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IStudentService _studentService;
        private readonly IEmployeeService _employeeService;
        private readonly ICanteenService _canteenService;

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IStudentService studentService,
            IEmployeeService employeeService,
            ICanteenService canteenService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _studentService = studentService;
            _employeeService = employeeService;
            _canteenService = canteenService;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserId);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Home/Index");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("Password", "User is locked out, please contact support");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password is incorrect");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserId", "User could not be found");
                }
            }
            return View(loginModel);
        }

        // logout
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }


        // -------------- student registration -------------------

        [AllowAnonymous]
        public IActionResult StudentRegister()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentRegister(StudentRegisterModel studentModel)
        {

            if (studentModel.StudentID != null)
            {
                var existingUser = await _userManager.FindByNameAsync(studentModel.StudentID);

                if (existingUser != null)
                    ModelState.AddModelError("StudentID", "This ID is already in use!");
            }

            if (ModelState.IsValid)
            {

                var user = new IdentityUser
                {
                    UserName = studentModel.StudentID!.ToString(),
                    EmailConfirmed = true
                };

                var Student = new Student()
                {
                    FirstName = studentModel.FirstName,
                    LastName = studentModel.LastName,
                    StudentID = studentModel.StudentID,
                    Email = studentModel.Email,
                    Phone = studentModel.Phone,
                    DOB = studentModel.DOB,
                    City = studentModel.City
                };

                var result = await _userManager.CreateAsync(user, studentModel.Password);
                await _userManager.AddClaimAsync(user, new Claim("Role", "Student"));

                if (result.Succeeded)
                {
                    await _studentService.AddStudent(Student);
                    await _signInManager.PasswordSignInAsync(user, studentModel.Password, false, false);
                    return RedirectToAction("Index", "Home");

                }
                return View(studentModel);
            }       

            return View(studentModel);

        }


        // --------------- employee registration -----------------

        [AllowAnonymous]
        public IActionResult EmployeeRegister()
        {
            _logger.LogInformation("Employee Register page called");

            var canteenList = _canteenService.GetAllCanteens();
            var model = new EmployeeRegisterModel
            {
                CanteenList = canteenList
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmployeeRegister(EmployeeRegisterModel employeeRegisterModel)
        {
            if (employeeRegisterModel.EmployeeID != null)
            {
                var existingUser = await _userManager.FindByNameAsync(employeeRegisterModel.EmployeeID);

                if (existingUser != null)
                    ModelState["EmployeeID"]?.Errors.Add("This ID is already in use!");
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("model state is valid");

                var user = new IdentityUser
                {
                    UserName = employeeRegisterModel.EmployeeID!.ToString(),
                    EmailConfirmed = true
                };

                var Employee = new Employee()
                {
                    FirstName = employeeRegisterModel.FirstName,
                    LastName = employeeRegisterModel.LastName,
                    EmployeeID = employeeRegisterModel.EmployeeID,
                    WorkPlace = employeeRegisterModel.WorkPlace.Value,
                };

                var result = await _userManager.CreateAsync(user, employeeRegisterModel.Password);
                await _userManager.AddClaimAsync(user, new Claim("Role", "Employee"));

                if (result.Succeeded)
                {
                    _logger.LogInformation("result has succeeded");

                    await _employeeService.AddEmployee(Employee);
                    await _signInManager.PasswordSignInAsync(user, employeeRegisterModel.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                return View(employeeRegisterModel);
            }

            _logger.LogInformation("modelstate is not valid");
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                if (modelStateVal.Errors.Count > 0)
                {
                    foreach (var error in modelStateVal.Errors)
                    {
                        _logger.LogInformation($"{modelStateKey}: {error.ErrorMessage}");
                    }
                }
            }


            return View(employeeRegisterModel);
        }


    }
}
