using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Spendnt.WEB.Auth
{
    public class AuthenticationProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimous = new ClaimsIdentity();
            var lffgUser = new ClaimsIdentity(new List<Claim>
        {
            new Claim("FirstName", "Luis"),
            new Claim("LastName", "Felipe"),
            new Claim(ClaimTypes.Name, "luisfelipe@gmail.com"),
            new Claim(ClaimTypes.Role, "Admin")
        },
                authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(lffgUser)));
        }
    }
}
