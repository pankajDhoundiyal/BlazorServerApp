using Blazored.LocalStorage;
using Blazored.SessionStorage;
using BlazorServeCrud.Pages.Admin;
using BlazorServeCrud.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorServeCrud.AuthProviders
{
    public class TestAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private readonly ILocalStorageService _localStorageService;
        private ClaimsPrincipal _principal;
        public TestAuthStateProvider(ISessionStorageService sessionStorageService, ILocalStorageService localStorageService)
        {
            _sessionStorageService = sessionStorageService;
            _localStorageService = localStorageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            
                var Email = await _sessionStorageService.GetItemAsync<string>("UserName");
                if (string.IsNullOrEmpty(Email))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, Email),
                    new Claim(ClaimTypes.Email, Email),
                }, "apiauth_type");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            
        }
        public void MarkUserAsAuthenticated(string email)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, email),
            }, "Custom Authentication");
            var user = new ClaimsPrincipal(identity);
            _principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }, "Custom Authentication"));
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        public void MarkUserAsLoggedOut()
        {
            // Close session
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
