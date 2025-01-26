using Microsoft.AspNetCore.Components.Authorization;
using Timesheet.Data;

namespace Timesheet.Services
{
    public class UserService
    {
        private AuthenticationStateProvider _stateProvider;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _stateProvider = authenticationStateProvider;

            _stateProvider.AuthenticationStateChanged += _stateProvider_AuthenticationStateChanged;
        }

        private async void _stateProvider_AuthenticationStateChanged(Task<AuthenticationState> task)
        {
            await GetUser();
        }

        public async Task<AppUser?> GetUser()
        {
            var authenticationState = await _stateProvider.GetAuthenticationStateAsync();

            if (authenticationState != null)
            {
                if (authenticationState.User.Identity == null)
                {
                    return null;
                }

                if (!authenticationState.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                var email = authenticationState.User.Claims.FirstOrDefault(x => x.Value.Contains("email"));

                var roles = authenticationState.User.Claims
                        .Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                        .Select(x => x.Value)
                        .ToList();

                if (email == null)
                {
                    return null;
                }

                return new AppUser
                {
                    Id = email.Value,
                    Name = authenticationState.User!.Identity!.Name!,
                    Email = email.Value,
                    Roles = roles
                };
            }
            else
            {
                return null;
            }
        }
    }
}
