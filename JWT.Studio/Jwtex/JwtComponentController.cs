using Jwt.Controller;
using System;
using System.Collections.Generic;
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
            string nameSpace = GetType().Assembly.GetName().Name;
            string index = "Jwtex.JwtResources.component.html";
            Response.ContentType = "text/html";
            using (Stream stream = GetType().Assembly.GetManifestResourceStream(index))
            {
                stream.CopyTo(Response.OutputStream);
            }

        }

        public JsonResult Download()
        {
            try
            {
                using (MemoryStream mem = GetURLContents("http://localhost:29324/Child/GetComponent?componentName=init"))
                {
                    using (ZipArchive arc = new ZipArchive(mem))
                    {
                        var path = Config.Root +"Scripts//Directives//init";
                        if (Directory.Exists(path))
                        {
                            RemoveDirectoryFiles(path);
                        }
                        arc.ExtractToDirectory(path);
                    }
                }
                return Json(new { msg = "installed successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
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
    
    }
}
