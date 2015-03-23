using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jwtSignalR.Startup))]
namespace jwtSignalR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
