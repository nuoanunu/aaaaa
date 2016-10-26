using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using System;
[assembly: OwinStartupAttribute(typeof(SSM.Startup))]
namespace SSM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);


        
        }
    }
}
