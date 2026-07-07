using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ipms.Frontend.Helpers
{
    public static class JwtHelper
    {
        public static string GetUserRole(string? token)
        {
            if (string.IsNullOrEmpty(token)) return string.Empty;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;
                return role ?? string.Empty;
            }
            catch
            {
                return string.Empty; 
            }
        }
        public static string GetCustomerId(string? token)
        {
            if (string.IsNullOrEmpty(token)) return string.Empty;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                return jwt.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                    ?.Value ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}