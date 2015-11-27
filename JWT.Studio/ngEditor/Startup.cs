using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(ngEditor.Startup))]
namespace ngEditor
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {          

            app.MapSignalR();

            
        }
    }
}
