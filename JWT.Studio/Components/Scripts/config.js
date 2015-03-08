
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/jwtFilter');

	stateprovider.state('root',{abstract:true,url:'/root',templateUrl:'Scripts/Layouts/root/root.html',controller:'rootCtrl as vm'});

	stateprovider.state('root.mac',{url:'/mac',templateUrl:'Scripts/Components/MacWidget/MacWidget.html',controller:'MacWidgetCtrl as vm'});
	stateprovider.state('root.jwtFilter',{url:'/jwtFilter',templateUrl:'Scripts/Components/jwtFilterWidget/jwtFilterWidget.html',controller:'jwtFilterWidgetCtrl as vm'});
	stateprovider.state('root.dog',{url:'/dog',templateUrl:'Scripts/Components/dogWidget/dogWidget.html',controller:'dogWidgetCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
