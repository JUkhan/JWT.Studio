//opath=F:\projects\JWT.Studio\jwtApp\Scripts\Services\DepartmentService.js,ab=true
using Jac.Entities.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Services
{
	[Angular(ModuleName = "app", ActionType = "factory", ActionName = "DepartmentService", DI = "$http,$q")]
	public class DepartmentService : BaseService<Department>
	{

		private HttpService http = null;
		private QService qService = null;
		public DepartmentService(HttpService http, QService qService):base("Department", http, qService)
		{
			this.http=http;
			this.qService=qService;
		}
		public Promise GetAdministratorList()
		{
			Deferred deffer = this.qService.defer();
			 this.http.get("{0}/GetAdministratorList".format(this.Root() + this.controllerName))
			.success((res, status) => { deffer.resolve(res); })
			.error((res, status) => { deffer.reject(res); });
			return deffer.promise;
		}
	}
}