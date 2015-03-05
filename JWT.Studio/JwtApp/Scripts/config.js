
export default function config(stateprovider, routeProvider){
	stateprovider.state('rootLayout',{abstract:true,url:'/rootLayout',templateUrl:'Templates/Layouts/rootLayout.html',controller:'rootLayoutCtrl as vm'});
	stateprovider.state('rootLayout.imsLayout',{abstract:true,url:'/imsLayout',templateUrl:'Templates/Layouts/imsLayout.html',controller:'imsLayoutCtrl as vm'});

	stateprovider.state('rootLayout.Course',{url:'/Course',templateUrl:'Templates/Widgets/Course.html',controller:'CourseCtrl as vm'});
	stateprovider.state('rootLayout.Student',{url:'/Student',templateUrl:'Templates/Widgets/Student.html',controller:'StudentCtrl as vm'});
	stateprovider.state('rootLayout.Enrollment',{url:'/Enrollment',templateUrl:'Templates/Widgets/Enrollment.html',controller:'EnrollmentCtrl as vm'});
	stateprovider.state('rootLayout.imsLayout.ims',{url:'/ims',views:{'col1':{templateUrl:'Templates/Widgets/Course.html',controller:'CourseCtrl as vm'},'col2':{templateUrl:'Templates/Widgets/Student.html',controller:'StudentCtrl as vm'},'col3':{templateUrl:'Templates/Widgets/Enrollment.html',controller:'EnrollmentCtrl as vm'}}});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
