using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteBanHang.Startup))]
namespace SiteBanHang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
