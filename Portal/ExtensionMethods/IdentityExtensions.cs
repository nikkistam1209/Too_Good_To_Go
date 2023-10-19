using System.Security.Claims;

namespace Portal.ExtensionMethods
{
    public static class IdentityExtensions
    {

        public static string GetRole(this ClaimsPrincipal user)
        {
            var isStudent = user.HasClaim("Role", "Student");
            var isEmployee = user.HasClaim("Role", "Employee");

            if (isStudent)
            {
                return "Student";
            }
            else if (isEmployee)
            {
                return "Employee";
            }
            else
            {
                return "None";
            }
        }

    }
}
