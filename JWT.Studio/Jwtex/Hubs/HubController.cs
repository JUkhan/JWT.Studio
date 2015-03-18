using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex.Hubs
{
    public class HubController<THub> : Jwt.Controller.BaseController where THub : IHub
    {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        protected IHubContext HUB
        {
            get { return hub.Value; }
        }
        public bool IsLock(FileInfo file)
        {
             return JwtHub.IsLock(file);
            
        }
    }
}
