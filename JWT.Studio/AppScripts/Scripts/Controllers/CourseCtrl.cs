
//opath=F:\projects\JWT.Studio\jwtApp\Scripts\Controllers\CourseCtrl.js,ab=true
using Jac.Entities.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Controllers
{
	public interface CourseScope : IBaseScope<Course>
	{
		List<Department> departmentList{get;set;}
	}
	[Angular(ModuleName = "app", ActionType = "controller", ActionName = "CourseCtrl", DI = "$scope,CourseService,$sce")]
	public class CourseCtrl : BaseController<Course>
	{

		private CourseScope scope = null;
		private CourseService service = null;
		public CourseCtrl(CourseScope scope, CourseService service, Sce sce):base(scope, service, sce)
		{
			this.scope=scope;
			this.service=service;
			this.scope.departmentList=new List<Department>();
			scope.gridOpts.columnDefs = new List<ColumnDef>{
				new ColumnDef{ name="AC",  width=50, enableSorting=false, cellTemplate="<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,new ColumnDef{ name="Title"}
				,new ColumnDef{ name="Credits"}
				,new ColumnDef{ name="Department", field="Department_Name"}
			};
			scope.gridOpts.onRegisterApi = gridApi => { 
				gridApi.paging.on.pagingChanged(scope,(newPage, pageSize)=>{
					this.pageNo = newPage;
					this.pageSize = pageSize;
					this.GetPaged();
				});
			};
			this.GetPaged();
			this.LoadRelationalData();
		}
		protected override void OnAfterDeleted(Course item)
		{
			this.scope.list.remove(x => x.CourseID == item.CourseID);
		}
		protected void LoadRelationalData()
		{
			this.service.GetDepartmentList().then(p =>
			{
				this.scope.departmentList=p.Data;
			});
		}
	}
}