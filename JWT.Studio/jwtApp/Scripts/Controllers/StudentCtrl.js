
(function(window){

var StudentCtrl=Scripts.Controllers.BaseController.extend({
scope:null,
service:null,
init:function(scope,service,sce)
{
this._super(scope, service, sce);
this.scope=scope;

this.service=service;

scope.gridOpts.columnDefs=[{name:"AC",width:50,enableSorting:false,cellTemplate:"<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>"},{name:"LastName"},{name:"FirstName"},{name:"EnrollmentDate",cellFilter:"date:'yyyy-MM-dd'"}];

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
this.scope.list.remove(function(x){return x.StudentID==item.StudentID;}.bind(this));


},
OnPreLoad:function(dataList)
{
dataList.ForEach(function(item){item.EnrollmentDate=this.ParseDateTime(item.EnrollmentDate);

}.bind(this));


return dataList;


},
OnBeforeAddInList:function(item)
{
item.EnrollmentDate=this.ParseDateTime(item.EnrollmentDate);


return item;


},
LoadRelationalData:function()
{

}});
namespace('Scripts.Controllers.StudentCtrl',StudentCtrl);

angular.module('app').controller('studentCtrl',['$scope','studentService','$sce', StudentCtrl]);
})(window);