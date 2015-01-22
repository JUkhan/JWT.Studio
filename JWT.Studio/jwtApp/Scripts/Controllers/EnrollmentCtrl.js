
(function(window){

var EnrollmentCtrl=Scripts.Controllers.BaseController.extend({
scope:null,
service:null,
init:function(scope,service,sce)
{
this._super(scope, service, sce);
this.scope=scope;

this.service=service;

this.scope.courseList=[];

this.scope.studentList=[];

scope.gridOpts.columnDefs=[{name:"AC",width:50,enableSorting:false,cellTemplate:"<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"},{name:"Course",field:"Course_Title"},{name:"Student",field:"Student_FirstName"},{name:"Grade"}];

scope.gridOpts.onRegisterApi=function(gridApi){gridApi.paging.on.pagingChanged(scope,function(newPage,pageSize){this.pageNo=newPage;

this.pageSize=pageSize;

this.GetPaged();

}.bind(this));

}.bind(this);

this.GetPaged();

this.LoadRelationalData();



},
OnAfterDeleted:function(item)
{
this.scope.list.remove(function(x){return x.EnrollmentID==item.EnrollmentID;}.bind(this));


},
LoadRelationalData:function()
{
this.service.GetCourseList().then(function(p){this.scope.courseList=p.Data;

}.bind(this));

this.service.GetStudentList().then(function(p){this.scope.studentList=p.Data;

}.bind(this));


}});
namespace('Scripts.Controllers.EnrollmentCtrl',EnrollmentCtrl);

angular.module('app').controller('EnrollmentCtrl',['$scope','EnrollmentService','$sce', EnrollmentCtrl]);
})(window);