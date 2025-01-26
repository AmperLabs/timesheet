using Microsoft.AspNetCore.Components.Authorization;

namespace Timesheet.Common
{
    public static class AuthenticationStateHelper
    {
        public static string? GetEmailForAuthenticatedUser(this AuthenticationState authenticationState)
        {
            var email = authenticationState.User.Claims.FirstOrDefault(x => x.Type.Contains("email"));

            return email?.Value;
        }

        public static string? GetNicknameForAuthenticatedUser(this AuthenticationState authenticationState)
        {
            var nickname = authenticationState.User.Claims.FirstOrDefault(x => x.Type.Contains("nickname"));

            return nickname?.Value;
        }

        public static string? GetPictureForAuthenticatedUser(this AuthenticationState authenticationState)
        {
            var picture = authenticationState.User.Claims.FirstOrDefault(x => x.Type.Contains("picture"));

            return picture?.Value;
        }

        public static List<string> GetRolesForAuthenticatedUser(this AuthenticationState authenticationState)
        {
            var roles = authenticationState.User.Claims
                        .Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                        .Select(x => x.Value)
                        .ToList();

            return roles ?? new List<string>();
        }

        public static List<string> GetClaimsForAuthenticatedUser(this AuthenticationState authenticationState)
        {
            var claims = authenticationState.User.Claims
                        .Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                        .Select(x => x.Value)
                        .ToList();

            return claims ?? new List<string>();
        }
    }
}
