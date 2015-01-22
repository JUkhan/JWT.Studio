
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
	public class StudentController : JwtController<Student, System.Int32>
	{

		//private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IStudentService service;
		public StudentController(IStudentService service)
		{
			this.service=service;
			SetService(service);
		}
	}
}