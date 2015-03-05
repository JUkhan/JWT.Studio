using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JwtApp2.Startup))]
namespace JwtApp2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
