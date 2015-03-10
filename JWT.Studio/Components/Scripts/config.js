
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/jwtFilter');

	stateprovider.state('root',{abstract:true,url:'/root',templateUrl:'Scripts/Layouts/root/root.html',controller:'rootCtrl as vm'});

	stateprovider.state('root.chart',{url:'/chart',templateUrl:'Scripts/Components/chartWidget/chartWidget.html',controller:'chartWidgetCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
