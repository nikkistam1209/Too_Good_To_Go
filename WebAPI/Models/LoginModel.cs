using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class LoginModel
    {
        public string? UserId { get; set; }

        public string? Password { get; set; }

    }
}
