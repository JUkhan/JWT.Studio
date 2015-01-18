//opath=F:\projects\JWT.Studio\jwtApp\Scripts\Services\CourseService.js,ab=true
using Jac.Entities.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Services
{
	[Angular(ModuleName = "app", ActionType = "factory", ActionName = "CourseService", DI = "$http,$q")]
	public class CourseService : BaseService<Course>
	{

		private HttpService http = null;
		private QService qService = null;
		public CourseService(HttpService http, QService qService):base("Course", http, qService)
		{
			this.http=http;
			this.qService=qService;
		}
		public Promise GetDepartmentList()
		{
			Deferred deffer = this.qService.defer();
			 this.http.get("{0}/GetDepartmentList".format(this.Root() + this.controllerName))
			.success((res, status) => { deffer.resolve(res); })
			.error((res, status) => { deffer.reject(res); });
			return deffer.promise;
		}
	}
}