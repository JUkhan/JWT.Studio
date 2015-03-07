﻿using Jwt.Controller;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Jwtex
{
    public class JwtComponentController : BaseController
    {
        public void Index()
        {
            //string nameSpace = GetType().Assembly.GetName().Name;
            string index = "Jwtex.JwtResources.component.html";

            Response.ContentType = "text/html";
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(index))
            {

                string data = StreamToString(stream).Replace("COMPONENT_SITE_URL", GetComponentUrl());

                StringToStream(data).CopyTo(Response.OutputStream);
            }

        }

        public JsonResult IsInstalled(string name)
        {
            DirectoryInfo dir = new DirectoryInfo(Config.Root + "Scripts//Directives");
            bool res = false;
            foreach (var item in dir.GetDirectories())
            {
                if (item.Name == name) { res = true; }
            }
            return Json(new { msg = res }, JsonRequestBehavior.AllowGet);
        }

        private string GetComponentUrl(){
            var url=ConfigurationManager.AppSettings["ComponentsUrl"]??"";
            return url.EndsWith("/") ? url : url + "/";
        }
        public JsonResult Download(string name)
        {
            try
            {
                using (MemoryStream mem = GetURLContents(GetComponentUrl() + "Child/GetComponent?componentName=" + name))
                {
                    using (ZipArchive arc = new ZipArchive(mem))
                    {
                        var path = Config.Root + "Scripts//Directives//" + name;
                        if (Directory.Exists(path))
                        {
                            RemoveDirectoryFiles(path);
                        }
                        arc.ExtractToDirectory(path);
                    }
                    UpdateAppDirectives();
                }
                return Json(new { msg = "installed successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        private void UpdateAppDirectives()
        {
            DirectoryInfo dir = new DirectoryInfo(Config.Root + "Scripts//Directives");
            StringBuilder import = new StringBuilder();
            StringBuilder builder = new StringBuilder();
            StringBuilder componentsCSS = new StringBuilder();
            foreach (var item in dir.GetDirectories())
            {
                import.AppendFormat("import {0} from 'Scripts/Directives/{0}/{0}.js';", item.Name);
                import.AppendLine();

                builder.AppendFormat(".directive('{0}', {0}.builder)", item.Name);
                builder.AppendLine();
                if (System.IO.File.Exists(string.Format(Config.Root+ "Scripts/Directives/{0}/{0}.css", item.Name)))
                {
                    componentsCSS.AppendFormat("@import '../Scripts/Directives/{0}/{0}.css';", item.Name);
                    componentsCSS.AppendLine();
                }
            }

            StringBuilder res = new StringBuilder();
            res.Append(import);
            res.AppendLine();
            res.AppendLine();
            res.Append("var moduleName='app.Directives';");
            res.AppendLine();
            res.AppendLine();
            res.Append("angular.module(moduleName, [])");
            res.AppendLine();
            res.Append(builder);
            res.Append(";");
            res.AppendLine();
            res.AppendLine();
            res.Append("export default moduleName;");
            System.IO.File.WriteAllText(Config.Root + "Scripts//app.directives.js", res.ToString());
            System.IO.File.WriteAllText(Config.Root + "Content//components.css", componentsCSS.ToString());
        }
        private void RemoveDirectoryFiles(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo file in dir.GetFiles()) file.Delete();
        }
        private MemoryStream GetURLContents(string url)
        {
            // The downloaded resource ends up in the variable named content. 
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL. 
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for 
            // the response. 
            // Note: you can't use HttpWebRequest.GetResponse in a Windows Store app. 
            using (WebResponse response = webReq.GetResponse())
            {

                // Get the data stream that is associated with the specified URL. 
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content.  
                    responseStream.CopyTo(content);
                }
            }

            // Return the result as a byte array. 
            return content;
        }
        private string StreamToString(Stream stream)
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