
(function(window){

var EnrollmentService=Scripts.Services.BaseService.extend({
http:null,
qService:null,
init:function(http,qService)
{
this._super("Enrollment", http, qService);
this.http=http;

this.qService=qService;



},
GetCourseList:function()
{
var deffer=this.qService.defer();
this.http.get("{0}/{1}/GetCourseList".format(this.Root(),this.controllerName)).success(function(res,status){deffer.resolve(res);

}.bind(this)).error(function(res,status){deffer.reject(res);

}.bind(this));


return deffer.promise;


},
GetStudentList:function()
{
var deffer=this.qService.defer();
this.http.get("{0}/{1}/GetStudentList".format(this.Root(),this.controllerName)).success(function(res,status){deffer.resolve(res);

}.bind(this)).error(function(res,status){deffer.reject(res);

}.bind(this));


return deffer.promise;


}});
namespace('Scripts.Services.EnrollmentService',EnrollmentService);

angular.module('app').factory('enrollmentService',['$http','$q',function(http,qService){ return new EnrollmentService(http,qService);}]);
})(window);