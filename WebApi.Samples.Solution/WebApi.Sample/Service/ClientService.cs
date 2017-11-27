using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WebApi.Sample.Service
{
    public class ClientService
    {
        private static List<string> _clients = new List<string>
        {
            "wkx","test"
        };
        public bool ValidateClient(string id)
        {
            return _clients.Contains(id);
        }
    }
}