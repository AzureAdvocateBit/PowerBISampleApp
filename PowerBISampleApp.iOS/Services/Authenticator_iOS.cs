﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PowerBISampleApp.iOS;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(Authenticator_iOS))]
namespace PowerBISampleApp.iOS
{
    public class Authenticator_iOS : IAuthenticator
    {
        public async Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri)
        {
            var authContext = new AuthenticationContext(authority);

            if (authContext.TokenCache?.ReadItems()?.Any() is true)
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);

            var uri = new Uri(returnUri);
            var controller = await MainThread.InvokeOnMainThreadAsync(Platform.GetCurrentUIViewController);
            var platformParams = new PlatformParameters(controller);

            return await authContext.AcquireTokenAsync(resource, clientId, uri, platformParams).ConfigureAwait(false);
        }
    }
}
