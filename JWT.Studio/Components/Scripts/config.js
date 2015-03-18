
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/chart');

	stateprovider.state('root',{abstract:true,url:'/root',templateUrl:'Scripts/Layouts/root/root.html',controller:'rootCtrl as vm'});

	stateprovider.state('root.chart',{url:'/chart',templateUrl:'Scripts/Components/chartWidget/chartWidget.html',controller:'chartWidgetCtrl as vm'});
	stateprovider.state('root.tableNav',{url:'/tableNav',templateUrl:'Scripts/Components/tableWidget/tableWidget.html',controller:'tableWidgetCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
