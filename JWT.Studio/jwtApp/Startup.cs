using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jwtApp.Startup))]
namespace jwtApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
