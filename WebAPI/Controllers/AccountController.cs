using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;

namespace WebAPI.Controllers
{
    //[Route("api/account")]
    //[ApiController]
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

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserId);
            if (user != null)
            {
                if ((await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                {
                    var securityTokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = (await _signInManager.CreateUserPrincipalAsync(user)).Identities.First(),
                        Expires = DateTime.Now.AddMinutes(int.Parse(_config["BearerTokens:ExpiryMinutes"])),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["BearerTokens:Key"])), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = new JwtSecurityTokenHandler().CreateToken(securityTokenDescriptor);

                    return Ok(new { StatusCode = (int)HttpStatusCode.OK, Message = "You have logged in successfully", Token = handler.WriteToken(securityToken) });
                }
            }
            return BadRequest(new { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Login incorrect" });
        }

        /*[HttpPost("signin")]
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
                        return Ok(new { StatusCode = 200, Token = token });
                    }
                    else
                    {
                        return BadRequest(new { StatusCode = 400, Message = "Password is incorrect" });
                    }
                }
                else
                {
                    return BadRequest(new { StatusCode = 400, Message = "User could not be found" });
                }
            }

            return BadRequest(ModelState);
        }

       
        private string GenerateJwtToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["BearerTokens:Key"]!));
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
                expires: DateTime.Now.AddMinutes(int.Parse(_config["BearerTokens:ExpiryMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/

    }
}
