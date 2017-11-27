using System;
using System.Collections.Concurrent;
using Microsoft.Owin.Security.Infrastructure;

namespace WebApi.Sample.Provider
{
    public class OpenAuthorizationCodeProvider:AuthenticationTokenProvider
    {
        private static readonly ConcurrentDictionary<string,string>
            _authenticationCodes=new ConcurrentDictionary<string, string>();

        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.SetToken(Guid.NewGuid().ToString("N")+Guid.NewGuid().ToString("N"));
            _authenticationCodes[context.Token] = context.SerializeTicket();
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            string token;
            if (_authenticationCodes.TryRemove(context.Token, out token))
            {
                context.DeserializeTicket(token);
            }
        }
    }
}