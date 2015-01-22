
//opath=F:\JWT.Studio\JWT.Studio\jwtApp\Scripts\Controllers\EnrollmentCtrl.js,ab=true
using EntityModule.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Controllers
{
	public interface EnrollmentScope : IBaseScope<Enrollment>
	{
		List<Course> courseList{get;set;}
		List<Student> studentList{get;set;}
	}
	[Angular(ModuleName = "app", ActionType = "controller", ActionName = "EnrollmentCtrl", DI = "$scope,EnrollmentService,$sce")]
	public class EnrollmentCtrl : BaseController<Enrollment>
	{

		private EnrollmentScope scope = null;
		private EnrollmentService service = null;
		public EnrollmentCtrl(EnrollmentScope scope, EnrollmentService service, Sce sce):base(scope, service, sce)
		{
			this.scope=scope;
			this.service=service;
			this.scope.courseList=new List<Course>();
			this.scope.studentList=new List<Student>();
			scope.gridOpts.columnDefs = new List<ColumnDef>{
				new ColumnDef{ name="AC",  width=50, enableSorting=false, cellTemplate="<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,new ColumnDef{ name="Course", field="Course_Title"}
				,new ColumnDef{ name="Student", field="Student_FirstName"}
				,new ColumnDef{ name="Grade"}
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
		protected override void OnAfterDeleted(Enrollment item)
		{
			this.scope.list.remove(x => x.EnrollmentID == item.EnrollmentID);
		}
		protected void LoadRelationalData()
		{
			this.service.GetCourseList().then(p =>
			{
				this.scope.courseList=p.Data;
			});
			this.service.GetStudentList().then(p =>
			{
				this.scope.studentList=p.Data;
			});
		}
	}
}