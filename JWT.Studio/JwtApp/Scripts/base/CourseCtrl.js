
import BaseCtrl from 'Scripts/Controllers/BaseCtrl.js';
const SVC=new WeakMap();
class CourseCtrl extends BaseCtrl
{

	constructor(courseSvc, scope, sce)
	{
		super(courseSvc, scope, sce);

		SVC.set(this, courseSvc);

		scope.gridOpts.columnDefs =[
			{ name:"AC",  width:50, enableSorting:false, cellTemplate:"<div style='text-align:center'><a ng-click=\"getExternalScopes().editAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().removeAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"}
				,{ name:"Title"}
				,{ name:"Credits"}
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
       	this.msg='as';
      	this.initFilter();
	}
	onAfterDeleted(item)
	{
		this.getScope().list.remove(x => x.CourseID == item.CourseID);
	}
	loadRelationalData()
	{
	}
}
CourseCtrl.$inject=['CourseSvc', '$scope', '$sce'];
export default CourseCtrl;