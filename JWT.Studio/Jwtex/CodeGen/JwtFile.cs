using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;
using System.Reflection;

namespace jwt.CodeGen
{
    public class JwtFile
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static object locker = new Object();

        public string Read(string path)
        {
            string res = "";
            lock (locker)
            {
                res = File.ReadAllText(path);
            }
            return res;
        }
        public void Write(string path, string content)
        {
            lock (locker)
            {
               File.WriteAllText(path, content);
            }
        }
        public List<string> GetFiles(string directoryName)
        {
            List<string> files = new List<string>();
            lock (locker)
            {
                foreach (var item in Directory.GetFiles(directoryName))
                {
                    files.Add(Path.GetFileName(item));
                }
            }
            return files;
        }
        public List<string> GetSubdirectories(string path)
        {           
            List<string> list = new List<string>();
            lock (locker)
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (var item in dir.GetDirectories())
                {
                    list.Add(item.Name);
                }
            }
            return list;
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
        public void CreateDirectory(string name)
        {
            if (!DirectoryExists(name))
            {
                Directory.CreateDirectory(name);
            }
        }

        public void RenameFile(string previousName, string newName)
        {
            lock (locker)
            {
                if (previousName == newName) return;
                if (FileExists(previousName))
                {
                    File.Copy(previousName, newName);
                    File.Delete(previousName);
                }
            }
        }
        public void RemoveFile(string fileName)
        {
            lock (locker)
            {
                if (FileExists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }
        public void Remove(string path)
        {
            lock (locker)
            {
                if (!DirectoryExists(path)) return;
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    foreach (var item in dir.GetFiles())
                    {
                        item.Delete();
                    }
                    dir.Delete();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }
        public void Rename(string path, string newName, string oldName)
        {
            lock (locker)
            {
                if (!DirectoryExists(path)) return;
                string path2 = path.Replace(oldName, newName);
                if (path != path2)
                    Directory.Move(path, path2);
                DirectoryInfo dir = new DirectoryInfo(path2);

                foreach (FileInfo item in dir.GetFiles())
                {
                    var temp = item.FullName.Substring(0, item.FullName.LastIndexOf(item.Name));
                    if (item.Name.Contains("Svc"))
                    {
                        temp += newName + "Svc" + item.Extension;
                    }
                    else if (item.Name.Contains("Ctrl"))
                    {
                        temp += newName + "Ctrl" + item.Extension;
                    }
                    else
                    {
                        temp += newName + item.Extension;
                    }
                    RenameFile(item.FullName, temp);
                    //replace into the file content
                    /* if (item.Name.Contains("Svc")||item.Name.Contains("Ctrl"))
                     {
                         var content= File.ReadAllText(temp);
                         File.WriteAllText(temp, content.Replace(oldName, newName));
                     }*/

                }
            }

        }

        public string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }
    }
}
