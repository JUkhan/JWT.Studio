
(function(window){

var CourseService=Scripts.Services.BaseService.extend({
http:null,
qService:null,
init:function(http,qService)
{
this._super("Course", http, qService);
this.http=http;

this.qService=qService;



}});
namespace('Scripts.Services.CourseService',CourseService);

angular.module('app').factory('courseService',['$http','$q',function(http,qService){ return new CourseService(http,qService);}]);
})(window);