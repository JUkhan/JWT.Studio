using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Jwtex
{
    public class JwtExController : Controller
    {
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
                string path = Server.MapPath("~");
                switch (directory)
                {
                    case "Controllers":
                    case "Services":
                        path += "Scripts\\" + directory;
                        setFiles(path,res, directory);
                        break;
                    case "Layouts":
                        path += "Templates\\Layouts" ;
                        setFiles(path, res, directory);
                        break;
                    case "Components":
                        path += "Templates\\Components";
                        setFiles(path, res, directory);
                        break;
                    case "Widgets":
                        path += "Templates\\Widgets" ;
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
        public JsonResult GetFileContent(string fileName, string key)
        {
            string path = Server.MapPath("~");
            JResult res = new JResult();
            try
            {
                switch (key)
                {
                    case "Controllers":
                        path += "Scripts\\Controllers\\" + fileName;
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                    case "Services":
                        path += "Scripts\\Services\\" + fileName;
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                    case "Layouts":
                        path += "Templates\\Layouts\\" + fileName;
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                    case "Components":
                        path += "Templates\\Components\\" + fileName;
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                    case "Widgets":
                        path += "Templates\\Widgets\\" + fileName;
                        res.data = System.IO.File.ReadAllText(path);
                        break;
                }
                res.isSuccess = true;

            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveFile(string key, string fileName, string content)
        {
            string path = Server.MapPath("~");
            JResult res = new JResult();
            try
            {
                switch (key)
                {
                    case "Controllers":
                        path += "Scripts\\Controllers\\" + fileName;
                        System.IO.File.WriteAllText(path, content);
                        break;
                    case "Services":
                        path += "Scripts\\Services\\" + fileName;
                        System.IO.File.WriteAllText(path, content);
                        break;
                    case "Layouts":
                        path += "Templates\\Layouts\\" + fileName;
                        System.IO.File.WriteAllText(path, content);
                        break;
                    case "Components":
                        path += "Templates\\Components\\" + fileName;
                        System.IO.File.WriteAllText(path, content);
                        break;
                    case "Widgets":
                        path += "Templates\\Widgets\\" + fileName;
                        System.IO.File.WriteAllText(path, content);
                        break;
                }
                res.isSuccess = true;
                res.msg = "Successfully saved.";
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }


        private List<string> GetFiles(string directoryName)
        {
            List<string> files = new List<string>();
            foreach (var item in Directory.GetFiles(directoryName))
            {
                files.Add(Path.GetFileName(item));
            }
            return files;
        }
    }
}
