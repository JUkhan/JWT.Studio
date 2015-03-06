using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Components.Startup))]
namespace Components
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
