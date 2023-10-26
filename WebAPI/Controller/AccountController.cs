using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;


        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserId);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

                    if (result.Succeeded)
                    {
                        var token = GenerateJwtToken(user);
                        return Ok(new { Token = token });
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Password is incorrect");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "User could not be found");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registerModel.StudentID,
                    Email = registerModel.Email,
                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {

                    var token = GenerateJwtToken(user);

                    return Ok(new { Token = token });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Registration", error.Description);
                    }
                }
            }

            return BadRequest(ModelState);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout successful");
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["BearerTokens:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName), 
            };

            var token = new JwtSecurityToken(
                issuer: _config["BearerTokens:Issuer"],
                audience: _config["BearerTokens:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_config["BearerTokens:ExpiryMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
