
import BaseCtrl from 'Scripts/Controllers/BaseCtrl.js';
const SVC=new WeakMap();
class EnrollmentCtrl extends BaseCtrl
{

	constructor(enrollmentSvc, scope, sce)
	{
		super(enrollmentSvc, scope, sce);

		SVC.set(this, enrollmentSvc);

		scope.gridOpts.columnDefs =[
			{ name:"AC",  width:50, enableSorting:false, cellTemplate:"<div style='text-align:center'><a ng-click=\"getExternalScopes().editAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().removeAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,{ name:"Course", field:"Course_Title"}
				,{ name:"Student", field:"Student_FirstName"}
				,{ name:"Grade"}
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
      	this.courseList=[];
	}
	onAfterDeleted(item)
	{
		this.getScope().list.remove(x => x.EnrollmentID == item.EnrollmentID);
	}
	loadRelationalData()
	{
		SVC.get(this).getCourseList().success(data =>
		{
			this.courseList=data; 
		});
		SVC.get(this).getStudentList().success(data =>
		{
			this.studentList=data;
		});
	}
}
EnrollmentCtrl.$inject=['EnrollmentSvc', '$scope', '$sce'];
export default EnrollmentCtrl;