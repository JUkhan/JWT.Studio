//opath=F:\JWT.Studio\JWT.Studio\jwtApp\Scripts\Services\StudentService.js,ab=true
using EntityModule.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Services
{
	[Angular(ModuleName = "app", ActionType = "factory", ActionName = "StudentService", DI = "$http,$q")]
	public class StudentService : BaseService<Student>
	{

		private HttpService http = null;
		private QService qService = null;
		public StudentService(HttpService http, QService qService):base("Student", http, qService)
		{
			this.http=http;
			this.qService=qService;
		}
	}
}