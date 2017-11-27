using System;
using System.Collections.Generic;
using System.Web.Http;

namespace WebApi.Sample.Controllers
{
    [Authorize]
    public class ValuesController:ApiController
    {
        [AllowAnonymous]
        public string T1()
        {
            return Guid.NewGuid().ToString("N");
        }

        public IEnumerable<string> Get()
        {
            return new[] {"value1","value2"};
        }

        public IEnumerable<string> Post()
        {
            return new[] {"v1","v2","v3"};
        }
    }
}