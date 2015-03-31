using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using System.Reflection;
using jwt.CodeGen;
using Jwtex.Hubs;
using System.Configuration;
using Jwt.Controller;

namespace Jwtex
{   

    public class JwtExController : BaseController //HubController<JwtHub>
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Index()
        {
            string nameSpace = GetType().Assembly.GetName().Name;
            string index = "Jwtex.JwtResources.index.html";
            Response.ContentType = "text/html";
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(index))
            {
                stream.CopyTo(Response.OutputStream);
            }

        }
       
        public void Resources(string file)
        {
            string ext = file.Substring(file.LastIndexOf(".") + 1);

            using (Stream stream = GetType().Assembly.GetManifestResourceStream("Jwtex.JwtResources." + file))
            {
                switch (ext)
                {
                    case "html":
                        Response.ContentType = "text/html";
                        break;
                    case "css":
                        Response.ContentType = "text/css";
                        break;
                    case "js":
                        Response.ContentType = "text/javascript";
                        break;
                }
                if (file == "app.js" || file == "workStatusApp.js")
                {
                    string data = StreamToString(stream).Replace("ROOT_PATH", GetRootPath());

                    StringToStream(data).CopyTo(Response.OutputStream);
                }
                else
                {
                    stream.CopyTo(Response.OutputStream);
                }
            }

        }
        private string GetRootPath()
        {
            var url = ConfigurationManager.AppSettings["RootPath"] ?? "";
            url = url.StartsWith("/") ? url : "/"+ url ;
            url= url.EndsWith("/") ? url : url + "/";
            return string.Format("rootPath:'{0}signalr',", url);
        }
        public JsonResult GetFileList(string directory)
        {
            JResult res = new JResult();
            try
            {
                string path = Config.Root;
                switch (directory)
                {
                    case "Controllers":
                    case "Services":
                        path += "Scripts\\" + directory;
                        setFiles(path, res, directory);
                        break;
                    case "Layouts":
                        path += "Templates\\Layouts";
                        setFiles(path, res, directory);
                        break;
                    case "Components":
                        path += "Templates\\Components";
                        setFiles(path, res, directory);
                        break;
                    case "Widgets":
                        path += "Templates\\Widgets";
                        setFiles(path, res, directory);
                        break;
                }

            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        private void setFiles(string path, JResult res, string directory)
        {
            JwtFile file = new JwtFile();
            if (!file.DirectoryExists(path))
            {
                res.msg = directory + " not exist";
            }
            else
            {
                res.data = file.GetFiles(path);
                res.isSuccess = true;
            }
        }
        public JsonResult IsFileExist(string mode, string directoryName, string fileName, string ext)
        {
            if (!fileName.EndsWith(ext)) fileName += ext;
            JwtFile file = new JwtFile();
            string path = Config.Root;
            switch (mode)
            {
                case "Base":
                    path += "Scripts\\Base\\" + fileName;
                    break;
                case "Widgets":
                    path += "Scripts\\Components\\" + directoryName+"\\" + fileName;
                    break;
                case "Layouts":
                    path += "Scripts\\Layouts\\" + directoryName + "\\" + fileName;
                    break;
                case "Components":
                    path += "Scripts\\Directives\\" + directoryName + "\\" + fileName;
                    break;
                case "Modules":
                    path += "Scripts\\Modules\\" + directoryName + "\\" + fileName;
                    break;
            }
            return Json(new { exist = file.FileExists(path) }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AddFile(string mode, string directoryName, string fileName, string ext)
        {
            if (!fileName.EndsWith(ext)) fileName += ext;
            string path = Config.Root;
            JwtFile file = new JwtFile();
            JResult res = new JResult();
            try
            {
                switch (mode)
                {
                    case "Base":                       
                        path += string.Format("Scripts\\Base\\{0}", fileName);
                       
                        break;

                    case "Layouts":
                        path += string.Format("Scripts\\Layouts\\{0}\\{1}", directoryName, fileName);
                       
                        break;
                    case "Components":
                        path += string.Format("Scripts\\Directives\\{0}\\{1}", directoryName, fileName);
                      
                        break;
                    case "Widgets":
                        path += string.Format("Scripts\\Components\\{0}\\{1}", directoryName, fileName);
                       
                        break;
                    case "Modules":
                        path += string.Format("Scripts\\Modules\\{0}\\{1}", directoryName, fileName);
                      
                        break;
                }
                file.Write(path, "new file");
                res.isSuccess = true;
                res.msg = "Alhumdulilla Successfully Created.";
            }
            catch (Exception ex)
            {
                
                log.Error(ex);
                res.msg = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveFile(string mode, string directoryName, string fileName, string ext)
        {
            if (!fileName.EndsWith(ext)) fileName += ext;
            string path = Config.Root;
            JwtFile file = new JwtFile();
            JResult res = new JResult();
            try
            {
                switch (mode)
                {
                    case "Base":
                        path += string.Format("Scripts\\Base\\{0}", fileName);

                        break;

                    case "Layouts":
                        path += string.Format("Scripts\\Layouts\\{0}\\{1}", directoryName, fileName);

                        break;
                    case "Components":
                        path += string.Format("Scripts\\Directives\\{0}\\{1}", directoryName, fileName);

                        break;
                    case "Widgets":
                        path += string.Format("Scripts\\Components\\{0}\\{1}", directoryName, fileName);

                        break;
                    case "Modules":
                        path += string.Format("Scripts\\Modules\\{0}\\{1}", directoryName, fileName);

                        break;
                }
                file.RemoveFile(path);
                res.isSuccess = true;
                res.msg = "Alhumdulilla Successfully Removed.";
            }
            catch (Exception ex)
            {

                log.Error(ex);
                res.msg = ex.Message;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFileContent(string mode, string directoryName, string fileName)
        {
            string path = Config.Root;
            JwtFile file = new JwtFile();
            JResult res = new JResult();
            try
            {
                switch (mode)
                {
                    case "Base":
                        directoryName = "base";
                        path += string.Format("Scripts\\Base\\{0}", fileName);
                        res.data = file.Read(path);// System.IO.File.ReadAllText(path);
                        break;

                    case "Layouts":
                        path += string.Format("Scripts\\Layouts\\{0}\\{1}", directoryName, fileName);
                        res.data = file.Read(path);
                        break;
                    case "Components":
                        path += string.Format("Scripts\\Directives\\{0}\\{1}", directoryName, fileName);
                        res.data = file.Read(path);
                        break;
                    case "Widgets":
                        path += string.Format("Scripts\\Components\\{0}\\{1}", directoryName, fileName);
                        res.data = file.Read(path);
                        break;
                    case "Modules":
                        path += string.Format("Scripts\\Modules\\{0}\\{1}", directoryName, fileName);
                        res.data = file.Read(path);
                        break;
                }
                res.isSuccess = true;
                res.locked = IsLock(new Hubs.FileInfo { Name = fileName, Folder = directoryName, Category = mode });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                res.msg = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveFile(string mode, string directoryName, string fileName, string content)
        {
            string path = Config.Root;
            JwtFile file = new JwtFile();
            JResult res = new JResult();
            try
            {
                switch (mode)
                {

                    case "Base":
                        path += string.Format("Scripts\\Base\\{0}", fileName);
                        file.Write(path, content);
                        break;
                    case "Layouts":
                        path += string.Format("Scripts\\Layouts\\{0}\\{1}", directoryName, fileName);
                        file.Write(path, content);
                        break;
                    case "Components":
                        path += string.Format("Scripts\\Directives\\{0}\\{1}", directoryName, fileName);
                        file.Write(path, content);
                        break;
                    case "Widgets":
                        path += string.Format("Scripts\\Components\\{0}\\{1}", directoryName, fileName);
                        file.Write(path, content);
                        break;
                    case "Modules":
                        path += string.Format("Scripts\\Modules\\{0}\\{1}", directoryName, fileName);
                        file.Write(path, content);
                        break;
                }
                res.isSuccess = true;
                res.msg = "Successfully saved.";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                res.msg = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #region New Style
        public JsonResult GetItems(string name)
        {
            JwtFile file = new JwtFile();
            List<string> list = new List<string>();
            switch (name)
            {

                case "Layouts":
                    list.Add("Select a layout");
                    list.AddRange(file.GetSubdirectories(Config.Root + "Scripts\\Layouts"));
                    break;
                case "Widgets":
                    list.Add("Select a widgets");
                    list.AddRange(file.GetSubdirectories(Config.Root + "Scripts\\Components"));
                    break;
                case "Components":
                    list.Add("Select a component");
                    list.AddRange(file.GetSubdirectories(Config.Root + "Scripts\\Directives"));

                    break;
                case "Modules":
                    list.Add("Select a Module");
                    list.AddRange(file.GetSubdirectories(Config.Root + "Scripts\\Modules"));

                    break;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemDetail(string name, string mode)
        {
            var asd = Request.ServerVariables;
            log.Info(string.Format("name:{0}, mode:{1}", name, mode));
            JwtFile file = new JwtFile();
            List<string> list = null;
            switch (mode)
            {
                case "Base":
                    list = file.GetFiles(Config.Root + "Scripts\\Base");
                    break;
                case "Layouts":
                    list = file.GetFiles(Config.Root + "Scripts\\Layouts\\" + name);
                    break;
                case "Widgets":
                    list = file.GetFiles(Config.Root + "Scripts\\Components\\" + name);
                    break;
                case "Components":
                    list = file.GetFiles(Config.Root + "Scripts\\Directives\\" + name);
                    break;
                case "Modules":
                    list = file.GetFiles(Config.Root + "Scripts\\Modules\\" + name);
                    break;
            }
            log.Info("files found: "+list.Count);
            return Json(new { js = list.Where(x => x.EndsWith(".js")), html = list.Where(x => x.EndsWith(".html")), css = list.Where(x => x.EndsWith(".css")) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsExist(string mode, string name)
        {
            JwtFile file = new JwtFile();
            string path = Config.Root;
            switch (mode)
            {
                case "Widgets":
                    path += "Scripts\\Components\\" + name;
                    break;
                case "Components":
                    path += "Scripts\\Derictives\\" + name;
                    break;
                case "Modules":
                    path += "Scripts\\Modules\\" + name;
                    break;
            }
            return Json(new { exist = file.DirectoryExists(path) }, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult CreateItem(string mode, string name)
        {
            try
            {
                CodeGen cg = new CodeGen();
                cg.Root = Config.Root;
                cg.CreateItem(mode, name);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(new { success = false, msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion
       
    }
}
