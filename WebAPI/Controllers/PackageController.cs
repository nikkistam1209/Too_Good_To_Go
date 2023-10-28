using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Core.DomainServices.IServices;
using Core.DomainServices.Services;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebAPI.GraphQL;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    public class PackageController : Controller
    {
        private readonly ILogger<PackageController> _logger;
        private readonly IPackageService _packageService;

        public PackageController(ILogger<PackageController> logger,
            IPackageService packageService)
        {
            _logger = logger;
            _packageService = packageService;
        }


        [Authorize]
        [HttpPost("api/packages/reserve/{id:int}")]
        public async Task<IActionResult> ReservePackage([FromRoute] int id)
        {
            try
            {
                await _packageService.ReservePackageAsync(id, this.User.Identity?.Name!);
                return Ok(new { StatusCode = 200, Message = "Package reserved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message= "Reservation failed: " + ex.Message });
            }
        }


    }
}
