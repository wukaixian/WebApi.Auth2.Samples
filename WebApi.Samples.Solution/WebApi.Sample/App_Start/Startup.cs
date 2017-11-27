using Microsoft.Owin;
using Owin;
using WebApi.Sample;

[assembly: OwinStartup(typeof(Startup))]
namespace WebApi.Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            ConfigurationAuth(app);
        }
    }
}
