using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using MusicClubManager.Abstractions;
using MusicClubManager.Blazor.Requirements;

namespace MusicClubManager.Blazor.Handlers
{
    public class ValidTokenHandler(AuthenticationStateProvider authenticationStateProvider, ITokenStore tokenStore) : AuthorizationHandler<ValidTokenRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidTokenRequirement requirement)
        {
            // Extract the token claims from the authorization context
            var user = context.User;
            var tokenExpirationClaim = user.FindFirst("exp")?.Value;

            if (tokenExpirationClaim != null && int.TryParse(tokenExpirationClaim, out var seconds))
            {
                var expirationDate = DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;

                if (expirationDate > DateTime.UtcNow)
                {
                    context.Succeed(requirement);
                }
                else
                {   //logout
                    await tokenStore.RemoveToken();

                    //Force the provider to get the state and notify everybody
                    await authenticationStateProvider.GetAuthenticationStateAsync();

                    context.Fail();
                }
            }
        }
    }
}
