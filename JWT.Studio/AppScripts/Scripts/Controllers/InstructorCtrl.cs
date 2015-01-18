
//opath=F:\projects\JWT.Studio\jwtApp\Scripts\Controllers\InstructorCtrl.js,ab=true
using Jac.Entities.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Controllers
{
	public interface InstructorScope : IBaseScope<Instructor>
	{
		List<OfficeAssignment> officeassignmentList{get;set;}
	}
	[Angular(ModuleName = "app", ActionType = "controller", ActionName = "InstructorCtrl", DI = "$scope,InstructorService,$sce")]
	public class InstructorCtrl : BaseController<Instructor>
	{

		private InstructorScope scope = null;
		private InstructorService service = null;
		public InstructorCtrl(InstructorScope scope, InstructorService service, Sce sce):base(scope, service, sce)
		{
			this.scope=scope;
			this.service=service;
			this.scope.officeassignmentList=new List<OfficeAssignment>();
			scope.gridOpts.columnDefs = new List<ColumnDef>{
				new ColumnDef{ name="AC",  width=50, enableSorting=false, cellTemplate="<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,new ColumnDef{ name="LastName"}
				,new ColumnDef{ name="FirstName"}
				,new ColumnDef{ name="HireDate" ,cellFilter="date:'yyyy-MM-dd'"}
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
		protected override void OnAfterDeleted(Instructor item)
		{
			this.scope.list.remove(x => x.InstructorID == item.InstructorID);
		}
		protected override List<Instructor> OnPreLoad(List<Instructor> dataList)
		{
			dataList.ForEach(item => {

			item.HireDate=this.ParseDateTime(item.HireDate);
			 });
			return dataList;
		}
		protected override Instructor OnBeforeAddInList(Instructor item)
		{

			item.HireDate=this.ParseDateTime(item.HireDate);
			return item;
		}
		protected void LoadRelationalData()
		{
			this.service.GetOfficeAssignmentList().then(p =>
			{
				this.scope.officeassignmentList=p.Data;
			});
		}
	}
}