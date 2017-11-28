using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.Sample.Filters;

namespace WebApi.Sample.Controllers
{
    [Authorize]
    public class ValuesController:ApiController
    {
        //[Allow]
        //public string T1()
        //{
        //    return Guid.NewGuid().ToString("N");
        //}
        
        [UserAuthorize]
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