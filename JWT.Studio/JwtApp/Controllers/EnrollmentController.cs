
using EntityModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Web.Mvc; 
using System.Web; 
//using log4net; 
using System.Reflection; 
using Services.Interfaces;
using Jwt.Core.Controllers; 
namespace WebApp.Controllers {
	public class EnrollmentController : JwtController<Enrollment, System.Int32>
	{

		//private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IEnrollmentService service;
		public EnrollmentController(IEnrollmentService service)
		{
			this.service=service;
			SetService(service);
		}
		public JsonResult GetCourseList()
		{
			 return Json(service.GetCourseList(), JsonRequestBehavior.AllowGet);
		}
		public JsonResult GetStudentList()
		{
			 return Json(service.GetStudentList(), JsonRequestBehavior.AllowGet);
		}
	}
}