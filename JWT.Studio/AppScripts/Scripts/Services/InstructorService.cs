//opath=F:\projects\JWT.Studio\jwtApp\Scripts\Services\InstructorService.js,ab=true
using Jac.Entities.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Services
{
	[Angular(ModuleName = "app", ActionType = "factory", ActionName = "InstructorService", DI = "$http,$q")]
	public class InstructorService : BaseService<Instructor>
	{

		private HttpService http = null;
		private QService qService = null;
		public InstructorService(HttpService http, QService qService):base("Instructor", http, qService)
		{
			this.http=http;
			this.qService=qService;
		}
		public Promise GetOfficeAssignmentList()
		{
			Deferred deffer = this.qService.defer();
			 this.http.get("{0}/GetOfficeAssignmentList".format(this.Root() + this.controllerName))
			.success((res, status) => { deffer.resolve(res); })
			.error((res, status) => { deffer.reject(res); });
			return deffer.promise;
		}
	}
}