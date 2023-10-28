using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class LoginModel
    {
        public string UserId { get; set; } = null!;

        public string Password { get; set; } = null!;

    }
}
