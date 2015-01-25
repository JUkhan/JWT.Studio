angular.module('app').config(['$stateProvider', '$urlRouterProvider', function (stateprovider, routeProvider) {
var root = '';
stateprovider.state('Student',{url:'/Student',templateUrl:root + 'Templates/Components/Student.html',controller:'StudentCtrl'});
stateprovider.state('Course',{url:'/Course',templateUrl:root + 'Templates/Components/Course.html',controller:'CourseCtrl'});
stateprovider.state('Enrollment',{url:'/Enrollment',templateUrl:root + 'Templates/Components/Enrollment.html',controller:'EnrollmentCtrl'});
}]);

jwt._arr={'Student':['Student',''],'Course':['Course',''],'Enrollment':['Enrollment','']};