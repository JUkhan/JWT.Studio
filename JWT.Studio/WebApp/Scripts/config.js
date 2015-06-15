
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/login');

	stateprovider.state('root',{abstract:true,url:'/root',templateUrl:'Scripts/Layouts/root/root.html',controller:'rootCtrl as vm'});

	stateprovider.state('root.home',{url:'/home',templateUrl:'Scripts/Components/home/home.html',controller:'homeCtrl as vm'});
	stateprovider.state('root.login',{url:'/login',templateUrl:'Scripts/Components/login/login.html',controller:'loginCtrl as vm'});
	stateprovider.state('root.signup',{url:'/signup',templateUrl:'Scripts/Components/signup/signup.html',controller:'signupCtrl as vm'});
	stateprovider.state('associate',{url:'/associate',templateUrl:'Scripts/Components/associate/associate.html',controller:'associateCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
