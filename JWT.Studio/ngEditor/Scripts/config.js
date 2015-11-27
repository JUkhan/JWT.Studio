
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/login');

	stateprovider.state('root',{url:'/root',templateUrl:'Scripts/Layouts/root/root.html',controller:'rootCtrl as vm'});

	stateprovider.state('root.max',{url:'/max',templateUrl:'Scripts/Components/test/test.html',controller:'testCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
