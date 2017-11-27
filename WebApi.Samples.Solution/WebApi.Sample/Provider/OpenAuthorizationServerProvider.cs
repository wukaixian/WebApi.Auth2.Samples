using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using WebApi.Sample.Service;

namespace WebApi.Sample.Provider
{
    public class OpenAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly UserService _userService=new UserService();
        private readonly ClientService _clientService=new ClientService();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            context.TryGetBasicCredentials(out clientId, out clientSecret);
            if (_clientService.ValidateClient(clientId))
            {
                context.SetError("invalid_client", "client is not valid");
            }

            context.Validated(clientId);
            await base.OnValidateClientAuthentication(context);
        }

        /// <summary>
        /// 客户端模式
        /// </summary>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity=new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name,"Client"));
            var ticket=new AuthenticationTicket(oAuthIdentity,new AuthenticationProperties());
            context.Validated(ticket);

            return base.GrantClientCredentials(context);
        }

        /// <summary>
        /// 密码模式
        /// </summary>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (string.IsNullOrEmpty(context.UserName))
            {
                context.SetError("invalid_username","username is not valid");
                return;
            }
            if (string.IsNullOrEmpty(context.Password))
            {
                context.SetError("invalid_password","password is not valid");
                return;
            }
            if (!_userService.ValidateUser(context.UserName,context.Password))
            {
                context.SetError("invalid_identity", "username or password is not valid");
                return;
            }

            var oAuthIdentity=new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name,context.UserName));

            context.Validated(oAuthIdentity);
        }

    }
}