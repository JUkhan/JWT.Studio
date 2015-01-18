//opath=F:\projects\JWT.Studio\jwtApp\Scripts\Services\EnrollmentService.js,ab=true
using Jac.Entities.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Services
{
	[Angular(ModuleName = "app", ActionType = "factory", ActionName = "EnrollmentService", DI = "$http,$q")]
	public class EnrollmentService : BaseService<Enrollment>
	{

		private HttpService http = null;
		private QService qService = null;
		public EnrollmentService(HttpService http, QService qService):base("Enrollment", http, qService)
		{
			this.http=http;
			this.qService=qService;
		}
		public Promise GetCourseList()
		{
			Deferred deffer = this.qService.defer();
			 this.http.get("{0}/GetCourseList".format(this.Root() + this.controllerName))
			.success((res, status) => { deffer.resolve(res); })
			.error((res, status) => { deffer.reject(res); });
			return deffer.promise;
		}
		public Promise GetStudentList()
		{
			Deferred deffer = this.qService.defer();
			 this.http.get("{0}/GetStudentList".format(this.Root() + this.controllerName))
			.success((res, status) => { deffer.resolve(res); })
			.error((res, status) => { deffer.reject(res); });
			return deffer.promise;
		}
	}
}