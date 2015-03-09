using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using System.Reflection;

namespace Jwtex
{
    public class JwtExController : Jwt.Controller.BaseController
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
                stream.CopyTo(Response.OutputStream);
            }

        }
        public JsonResult GetFileList(string directory)
        {
            JResult res = new JResult();
            try
            {
                string path =Config.Root;
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
            if (!Directory.Exists(path))
            {
                res.msg = directory + " not exist";
            }
            else
            {
                res.data = GetFiles(path);
                res.isSuccess = true;
            }
        }
        public JsonResult GetFileContent(string mode, string directoryName, string fileName)
        {
            string path = Config.Root;
            JResult res = new JResult();
            try
            {
                switch (mode)
                {
                   
                    case "Layouts":
                        path += string.Format("Scripts\\Layouts\\{0}\\{1}", directoryName, fileName);
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                    case "Components":
                        path += string.Format("Scripts\\Directives\\{0}\\{1}", directoryName, fileName);
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                    case "Widgets":
                        path += string.Format("Scripts\\Components\\{0}\\{1}", directoryName, fileName);
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                }
                res.isSuccess = true;

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
            JResult res = new JResult();
            try
            {
                switch (mode)
                {                    
                    
                    case "Layouts":
                        path += string.Format("Scripts\\Layouts\\{0}\\{1}", directoryName, fileName);
                        System.IO.File.WriteAllText(path, content);
                        break;
                    case "Components":
                        path += string.Format("Scripts\\Directives\\{0}\\{1}", directoryName, fileName);
                        System.IO.File.WriteAllText(path, content);
                        break;
                    case "Widgets":
                        path += string.Format("Scripts\\Components\\{0}\\{1}", directoryName, fileName);
                        System.IO.File.WriteAllText(path, content);
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
            List<string> list = new List<string>();
            switch (name)
            {
                case "Layouts":
                    list.Add("Select a layout");
                    list.AddRange(GetSubdirectories(Config.Root + "Scripts\\Layouts"));
                    break;
                case "Widgets":
                    list.Add("Select a widgets");
                    list.AddRange(GetSubdirectories(Config.Root + "Scripts\\Components"));
                    break;
                case "Components":
                    list.Add("Select a component");
                    list.AddRange(GetSubdirectories(Config.Root + "Scripts\\Directives"));

                    break;
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemDetail(string name, string mode)
        {
            List<string> list = null;
            switch (mode)
            {
                case "Layouts":
                    list = GetFiles(Config.Root + "Scripts\\Layouts\\" + name);
                    break;
                case "Widgets":
                    list = GetFiles(Config.Root + "Scripts\\Components\\" + name);
                    break;
                case "Components":

                    list = GetFiles(Config.Root + "Scripts\\Directives\\" + name);

                    break;
            }
            return Json(new {  js=list.Where(x=>x.EndsWith(".js")), html=list.Where(x=>x.EndsWith(".css")||x.EndsWith(".html"))}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsExist(string mode, string name)
        {
            string path = Config.Root;
            switch (mode)
            {
                case "Widgets":
                    path += "Scripts\\Components\\" + name;
                    break;
                case "Components":
                    path += "Scripts\\Derictives\\" + name;
                    break;
            }
            return Json(new { exist = Directory.Exists(path) }, JsonRequestBehavior.AllowGet);
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
        private List<string> GetFiles(string directoryName)
        {
            List<string> files = new List<string>();
            foreach (var item in Directory.GetFiles(directoryName))
            {
                files.Add(Path.GetFileName(item));
            }
            return files;
        }
        private List<string> GetSubdirectories(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> list = new List<string>();
            foreach (var item in dir.GetDirectories())
            {
                list.Add(item.Name);
            }
            return list;
        }
    }
}
