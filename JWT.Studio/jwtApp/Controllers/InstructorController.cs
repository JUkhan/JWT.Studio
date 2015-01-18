
using Jac.Entities.Entities;
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
	public class InstructorController : JwtController<Instructor, System.Int32>
	{

		//private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IInstructorService service;
		public InstructorController(IInstructorService service)
		{
			this.service=service;
			SetService(service);
		}
		public JsonResult GetOfficeAssignmentList()
		{
			 return Json(service.GetOfficeAssignmentList(), JsonRequestBehavior.AllowGet);
		}
	}
}