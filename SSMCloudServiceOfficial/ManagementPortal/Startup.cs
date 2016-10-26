using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManagementPortal.Startup))]
namespace ManagementPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
