export default function config(stateprovider, routeProvider){
var root = '';
stateprovider.state('nav1',{url:'/nav1',templateUrl:root + 'Templates/Components/nav1.html',controller:'nav1Ctrl as vm'});
stateprovider.state('layout1',{abstract:true, url:'/layout1',templateUrl: root + 'Templates/Layouts/layout1.html'});
stateprovider.state('nav2',{url:'/nav2',templateUrl:root + 'Templates/Components/nav2.html',controller:'nav2Ctrl as vm'});
stateprovider.state('layout1.max',{url:'/max',views:{'col1':{controller:'StudentCtrl as vm', templateUrl:root + 'Templates/Components/Student.html'},'col2':{controller:'CourseCtrl as vm', templateUrl:root + 'Templates/Components/Course.html'}}});
stateprovider.state('Course',{url:'/Course',templateUrl:root + 'Templates/Components/Course.html',controller:'CourseCtrl as vm'});
stateprovider.state('Student',{url:'/Student',templateUrl:root + 'Templates/Components/Student.html',controller:'StudentCtrl as vm'});
stateprovider.state('Enrollment',{url:'/Enrollment',templateUrl:root + 'Templates/Components/Enrollment.html',controller:'EnrollmentCtrl as vm'});
}

config.$inject=['$stateProvider', '$urlRouterProvider'];