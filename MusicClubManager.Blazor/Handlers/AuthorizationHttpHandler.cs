using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MusicClubManager.Abstractions;
using MusicClubManager.Sdk.Extensions;
using System.Net;
using System.Net.Http.Json;
using System.Text;


namespace MusicClubManager.Blazor.Handlers
{
    public class AuthorizationHttpHandler(ITokenStore tokenStore, NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri?.AbsoluteUri.Equals("https://localhost:7188/Identity/Token") is false && (await authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity?.IsAuthenticated is false)
            {
                navigationManager.NavigateTo($"/sign-in?returnUrl={navigationManager.ToBaseRelativePath(navigationManager.Uri)}");

                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            var token = await tokenStore.GetAccessToken();

            request.AddAuthorization(token);

            var response = await base.SendAsync(request, cancellationToken);

            //if (response.StatusCode == HttpStatusCode.Unauthorized)
            //{
            //    navigationManager.NavigateTo($"/sign-in?returnUrl={navigationManager.ToBaseRelativePath(navigationManager.Uri)}");
            //}

            return response;
        }
    }
}
