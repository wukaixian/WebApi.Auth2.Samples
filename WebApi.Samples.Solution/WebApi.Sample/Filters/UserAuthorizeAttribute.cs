using System;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Sample.Filters
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class UserAuthorizeAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext==null)
            {
                throw new ArgumentNullException("actionContext is null");
            }
            if (!IsAuthorized(actionContext))
            {
                //reject the request
                HandleUnauthorizedRequest(actionContext);
            }
            //pass
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            
            if (identity.Name!="wkx")
            {
                return false;
            }
            return true;
        }
    }
}