
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/jwtFilter');

	stateprovider.state('root',{abstract:true,url:'/root',templateUrl:'Templates/Layouts/root.html',controller:'rootCtrl as vm'});

	stateprovider.state('root.mac',{url:'/mac',templateUrl:'Templates/Widgets/MacWidget.html',controller:'MacWidgetCtrl as vm'});
	stateprovider.state('root.jwtFilter',{url:'/jwtFilter',templateUrl:'Templates/Widgets/jwtFilterWidget.html',controller:'jwtFilterWidgetCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
