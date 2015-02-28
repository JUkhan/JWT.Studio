
import BaseCtrl from 'Scripts/Controllers/BaseCtrl.js';
const SVC=new WeakMap();
class StudentCtrl extends BaseCtrl
{

	constructor(studentSvc, scope, sce)
	{
		super(studentSvc, scope, sce);

		SVC.set(this, studentSvc);

		scope.gridOpts.columnDefs =[
			{ name:"AC",  width:50, enableSorting:false, cellTemplate:"<div style='text-align:center'><a ng-click=\"getExternalScopes().editAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().removeAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,{ name:"LastName"}
				,{ name:"FirstName"}
				,{ name:"EnrollmentDate" ,cellFilter:"jwtDate | date:'yyyy-MM-dd'"}
			];
			scope.gridOpts.onRegisterApi = gridApi => { 
				gridApi.paging.on.pagingChanged(scope,(newPage, pageSize)=>{
					this.pageNo = newPage;
					this.pageSize = pageSize;
					this.getPagedList();
				});
			};
		this.getPagedList();
		this.loadRelationalData();
	}
	onAfterDeleted(item)
	{
		this.getScope().list.remove(x => x.StudentID == item.StudentID);
	}
	loadRelationalData()
	{
	}
}
StudentCtrl.$inject=['StudentSvc', '$scope', '$sce'];
export default StudentCtrl;