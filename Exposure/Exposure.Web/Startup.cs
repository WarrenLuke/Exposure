using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Exposure.Web.Startup))]
namespace Exposure.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
