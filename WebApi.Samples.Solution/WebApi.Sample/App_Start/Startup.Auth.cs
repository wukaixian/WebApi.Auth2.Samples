using System;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApi.Sample.Provider;

namespace WebApi.Sample
{
    public partial class Startup
    {
        public void ConfigurationAuth(IAppBuilder app)
        {
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                AuthenticationMode = AuthenticationMode.Active,
                TokenEndpointPath = new PathString("/api/token"), 
                AuthorizeEndpointPath = new PathString("/api/authorize"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(3), 
                Provider = new OpenAuthorizationServerProvider(),
                //AuthorizationCodeProvider = new OpenAuthorizationCodeProvider(),
                RefreshTokenProvider = new OpenRefreshTokenProvider() 
            };

            app.UseOAuthBearerTokens(oAuthOptions);
        }
    }
}