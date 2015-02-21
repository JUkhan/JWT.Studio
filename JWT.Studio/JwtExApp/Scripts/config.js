
export default function config(stateprovider, routeProvider){
	stateprovider.state('layout1',{abstract:true,url:'/layout1',templateUrl:'Templates/Layouts/layout1.html'});

	stateprovider.state('nav1',{url:'/nav1',templateUrl:'Templates/Widgets/DbWeather.html',controller:'DbWeatherCtrl as vm'});
	stateprovider.state('nav2',{url:'/nav2',templateUrl:'Templates/Widgets/testWidget.html',controller:'testWidgetCtrl as vm'});
	stateprovider.state('layout1.nav3',{url:'/nav3',views:{'view1':{templateUrl:'Templates/Widgets/DbWeather.html',controller:'DbWeatherCtrl as vm'},'view2':{templateUrl:'Templates/Widgets/testWidget.html',controller:'testWidgetCtrl as vm'}}});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
