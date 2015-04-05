using Jwt.Controller;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Jwtex.Hubs
{
    [HubName("jwt")]

    public class JwtHub : Hub
    {
        private static ConcurrentDictionary<string, List<FileInfo>> _mapping = new ConcurrentDictionary<string, List<FileInfo>>();
        private static object locker = new Object();
       

        public override Task OnConnected()
        {           
            return base.OnConnected();
        }
        public void InitHub(string user)
        {
            _mapping.TryAdd(Context.ConnectionId, new List<FileInfo>() { new FileInfo { UserName = user, CID = Context.ConnectionId } });

            Clients.Others.newConnection(user);
            Clients.Caller.onlineUsers(GetAllUsers(user));
            GetWorkStatus();
        }
        private List<string> GetAllUsers(string user)
        {
            List<string> list = new List<string>();
            foreach (var item in _mapping.Values)
            {
                if (item[0].UserName != user)
                {
                    list.Add(item[0].UserName);
                }
            }
            return list;
        }
        public void SendMessage(string sender, string sendto, string message)
        {
            var cid = GetConnectionID(sendto);
            if (!string.IsNullOrEmpty(cid))
            {
                Clients.Client(cid).receiveMessage(new { sender = sender, message = message });
            }
            else
            {
                Clients.Caller.receiveMessage(new { sender = "Server", message = "Sorry! Person is not available." });
            }
        }
        private string GetConnectionID(string user)
        {
            foreach (var item in _mapping.Values)
            {
                if (item[0].UserName == user) return item[0].CID;
            }
            return null;
        }
        public void GetWorkStatus()
        {
            Clients.Caller.workStatus(Deserialize());
        }
        public void Lock(FileInfo info)
        {
            info.CID = Context.ConnectionId;
            info.UserName = info.UserName;
            info.Lock = true;
            info.Start = DateTime.Now.ToLongTimeString();
            info.End = "";
            Clients.Others.lockFile(info);
            _mapping[info.CID].Add(info);
            Persist(info);
        }

        private void Persist(FileInfo info)
        {
            var list = Deserialize();
            list.Add(info);
            Serialize(list);
        }
        public void Unlock(FileInfo info)
        {
            var item = _mapping[Context.ConnectionId]
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
            if (item != null)
            {
                info.UserName = item.UserName;
                info.Start = "";
                info.End = DateTime.Now.ToLongTimeString();
                UnlockHelper(info);
                Persist(info);
                _mapping[Context.ConnectionId].Remove(item);
            }

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
                string user = _mapping[Context.ConnectionId][0].UserName;
                _mapping.TryRemove(Context.ConnectionId, out list);
                Clients.All.removeConnection(user);
            }
            return base.OnDisconnected(stopCalled);
        }

        public static bool IsLock(FileInfo info)
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
        // file serialize
        public void Serialize(List<FileInfo> list)
        {

            lock (locker)
            {
                string path = BaseController.ROOTPATH + "jwtws";
                CreateDirectory(path);
                XmlSerializer serializer = new XmlSerializer(typeof(List<FileInfo>));
                using (TextWriter writer = new StreamWriter(path + "\\" + GetFileName() + ".config"))
                {
                    serializer.Serialize(writer, list);
                }
            }

        }

        public List<FileInfo> Deserialize()
        {
            lock (locker)
            {
                List<FileInfo> temp = new List<FileInfo>();
                try
                {
                    string path = BaseController.ROOTPATH + "jwtws" + "\\" + GetFileName() + ".config";
                    if (!File.Exists(path)) { return temp; }
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<FileInfo>));
                    using (TextReader reader = new StreamReader(path))
                    {
                        temp = (List<FileInfo>)deserializer.Deserialize(reader);

                    }
                }
                catch { }

                return temp;
            }

        }
        private string GetFileName()
        {
            return DateTime.Now.ToString("ddMMyyyy");
        }
        private void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
    }
}
