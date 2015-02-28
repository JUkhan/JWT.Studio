
export default function config(stateprovider, routeProvider){

	stateprovider.state('Course',{url:'/Course',templateUrl:'Templates/Widgets/Course.html',controller:'CourseCtrl as vm'});
	stateprovider.state('Student',{url:'/Student',templateUrl:'Templates/Widgets/Student.html',controller:'StudentCtrl as vm'});
	stateprovider.state('Enrollment',{url:'/Enrollment',templateUrl:'Templates/Widgets/Enrollment.html',controller:'EnrollmentCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
