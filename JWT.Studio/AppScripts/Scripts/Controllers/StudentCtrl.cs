
//opath=F:\JWT.Studio\JWT.Studio\jwtApp\Scripts\Controllers\StudentCtrl.js,ab=true
using EntityModule.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Controllers
{
	public interface StudentScope : IBaseScope<Student>
	{
	}
	[Angular(ModuleName = "app", ActionType = "controller", ActionName = "StudentCtrl", DI = "$scope,StudentService,$sce")]
	public class StudentCtrl : BaseController<Student>
	{

		private StudentScope scope = null;
		private StudentService service = null;
		public StudentCtrl(StudentScope scope, StudentService service, Sce sce):base(scope, service, sce)
		{
			this.scope=scope;
			this.service=service;
			scope.gridOpts.columnDefs = new List<ColumnDef>{
				new ColumnDef{ name="AC",  width=50, enableSorting=false, cellTemplate="<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,new ColumnDef{ name="LastName"}
				,new ColumnDef{ name="FirstName"}
				,new ColumnDef{ name="EnrollmentDate" ,cellFilter="jwtDate | date:'yyyy-MM-dd'"}
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
		protected override void OnAfterDeleted(Student item)
		{
			this.scope.list.remove(x => x.StudentID == item.StudentID);
		}
		protected void LoadRelationalData()
		{
		}
	}
}