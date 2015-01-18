
//opath=F:\projects\JWT.Studio\jwtApp\Scripts\Controllers\DepartmentCtrl.js,ab=true
using Jac.Entities.Entities;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
using Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Scripts.Controllers
{
	public interface DepartmentScope : IBaseScope<Department>
	{
		List<Instructor> administratorList{get;set;}
	}
	[Angular(ModuleName = "app", ActionType = "controller", ActionName = "DepartmentCtrl", DI = "$scope,DepartmentService,$sce")]
	public class DepartmentCtrl : BaseController<Department>
	{

		private DepartmentScope scope = null;
		private DepartmentService service = null;
		public DepartmentCtrl(DepartmentScope scope, DepartmentService service, Sce sce):base(scope, service, sce)
		{
			this.scope=scope;
			this.service=service;
			this.scope.administratorList=new List<Instructor>();
			scope.gridOpts.columnDefs = new List<ColumnDef>{
				new ColumnDef{ name="AC",  width=50, enableSorting=false, cellTemplate="<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,new ColumnDef{ name="Name"}
				,new ColumnDef{ name="Budget"}
				,new ColumnDef{ name="StartDate" ,cellFilter="date:'yyyy-MM-dd'"}
				,new ColumnDef{ name="Administrator", field="Administrator_FirstName"}
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
		protected override void OnAfterDeleted(Department item)
		{
			this.scope.list.remove(x => x.DepartmentID == item.DepartmentID);
		}
		protected override List<Department> OnPreLoad(List<Department> dataList)
		{
			dataList.ForEach(item => {

			item.StartDate=this.ParseDateTime(item.StartDate);
			 });
			return dataList;
		}
		protected override Department OnBeforeAddInList(Department item)
		{

			item.StartDate=this.ParseDateTime(item.StartDate);
			return item;
		}
		protected void LoadRelationalData()
		{
			this.service.GetAdministratorList().then(p =>
			{
				this.scope.administratorList=p.Data;
			});
		}
	}
}