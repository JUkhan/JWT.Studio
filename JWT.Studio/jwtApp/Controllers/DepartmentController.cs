
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
	public class DepartmentController : JwtController<Department, System.Int32>
	{

		//private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IDepartmentService service;
		public DepartmentController(IDepartmentService service)
		{
			this.service=service;
			SetService(service);
		}
		public JsonResult GetAdministratorList()
		{
			 return Json(service.GetAdministratorList(), JsonRequestBehavior.AllowGet);
		}
	}
}