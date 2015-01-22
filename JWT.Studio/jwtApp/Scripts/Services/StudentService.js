
(function(window){

var StudentService=Scripts.Services.BaseService.extend({
http:null,
qService:null,
init:function(http,qService)
{
this._super("Student", http, qService);
this.http=http;

this.qService=qService;



}});
namespace('Scripts.Services.StudentService',StudentService);

angular.module('app').factory('StudentService',['$http','$q',function(http,qService){ return new StudentService(http,qService);}]);
})(window);