using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Components.Controllers
{
    public class ChildController : Controller
    {
        //
        // GET: /Child/
        public ActionResult Index()
        {
            return View();
        }

        public void GetComponent(string componentName)
        {
            string SOURCE = Server.MapPath("~") + "Scripts/Directives/" + componentName;
            string DESTINATION = Server.MapPath("~") + componentName + ".zip";
            if (System.IO.File.Exists(DESTINATION))
            {
                System.IO.File.Delete(DESTINATION);
            }
            ZipFile.CreateFromDirectory(SOURCE, DESTINATION);
            Response.TransmitFile(DESTINATION);

        }

    }
}