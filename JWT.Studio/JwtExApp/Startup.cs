using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JwtExApp.Startup))]
namespace JwtExApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
