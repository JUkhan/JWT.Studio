using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JwtExApp.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Data1()
        {
            return Json(new { id = 101, msg = "first data. +" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Data2(int id)
        {
            return Json(new { id = id, msg = "second data." }, JsonRequestBehavior.AllowGet);
        }
	}
}