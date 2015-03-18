using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwtex.Hubs
{
    [HubName("jwt")]
    public class JwtHub : Hub
    {
        private static ConcurrentDictionary<string, List<FileInfo>> _mapping = new ConcurrentDictionary<string, List<FileInfo>>();


        public override Task OnConnected()
        {
            _mapping.TryAdd(Context.ConnectionId, new List<FileInfo>());
            Clients.All.newConnection(Context.ConnectionId);
            return base.OnConnected();
        }

        public void Lock(FileInfo info)
        {
            info.Lock = true;
            Clients.Others.lockFile(info);
            _mapping[Context.ConnectionId].Add(info);
        }

        public void Unlock(FileInfo info)
        {
          var item=  _mapping[Context.ConnectionId]
                .Find(file => file.Category == info.Category
                && file.Name == info.Name
                && file.Folder == info.Folder);
          if (item == null && info.Category == "Base")
          {
              item = _mapping[Context.ConnectionId]
                .Find(file => file.Category == info.Category
                && file.Name == info.Name
               );
          }
            UnlockHelper(info);
            _mapping[Context.ConnectionId].Remove(item);
        }

        private void UnlockHelper(FileInfo info)
        {

            Clients.Others.unlockFile(info);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (_mapping.ContainsKey(Context.ConnectionId))
            {
                foreach (var info in _mapping[Context.ConnectionId])
                {
                    if (info.Lock)
                    {
                        UnlockHelper(info);
                    }
                }              
               
                var list = new List<FileInfo>();
                _mapping.TryRemove(Context.ConnectionId, out list);
                Clients.All.removeConnection(Context.ConnectionId);
            }
            return base.OnDisconnected(stopCalled);
        }

        public static  bool IsLock(FileInfo info)
        {
            foreach (var item in _mapping)
            {
                var temp = item.Value
                .Find(file => file.Category == info.Category
                && file.Name == info.Name
                && file.Folder == info.Folder);
                if (temp != null && temp.Lock == true) { return true; }
            }         
            
            return false;
        }
    }
}
