
export default function config(stateprovider, routeProvider){
	stateprovider.state('rootLayout',{abstract:true,url:'/rootLayout',templateUrl:'Scripts/Layouts/rootLayout/rootLayout.html',controller:'rootLayoutCtrl as vm'});
	stateprovider.state('rootLayout.imsLayout',{abstract:true,url:'/imsLayout',templateUrl:'Scripts/Layouts/imsLayout/imsLayout.html',controller:'imsLayoutCtrl as vm'});

	stateprovider.state('rootLayout.Course',{url:'/Course',templateUrl:'Scripts/Components/Course/Course.html',controller:'CourseCtrl as vm'});
	stateprovider.state('rootLayout.Student',{url:'/Student',templateUrl:'Scripts/Components/Student/Student.html',controller:'StudentCtrl as vm'});
	stateprovider.state('rootLayout.Enrollment',{url:'/Enrollment',templateUrl:'Scripts/Components/Enrollment/Enrollment.html',controller:'EnrollmentCtrl as vm'});
	stateprovider.state('rootLayout.imsLayout.ims',{url:'/ims',views:{'col1':{templateUrl:'Scripts/Components/Course/Course.html',controller:'CourseCtrl as vm'},'col2':{templateUrl:'Scripts/Components/Student/Student.html',controller:'StudentCtrl as vm'},'col3':{templateUrl:'Scripts/Components/Enrollment/Enrollment.html',controller:'EnrollmentCtrl as vm'}}});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
