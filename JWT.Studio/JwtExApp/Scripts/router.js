angular.module('app').config(['$stateProvider', '$urlRouterProvider', function (stateprovider, routeProvider) {

	stateprovider.state('layout1',{abstract:true,url:'/layout1',templateUrl:'Templates/Layouts/layout1.html'});
	stateprovider.state('layout2',{abstract:true,url:'/layout2',templateUrl:'Templates/Layouts/layout2.html'});

	stateprovider.state('layout1.nav1',{url:'/nav1',views:{'col1':{templateUrl:'Templates/Widgets/widget1/widget1.html',controller:'widget1Ctrl'},'col2':{templateUrl:'Templates/Widgets/widget1/widget1.html',controller:'widget1Ctrl'}}});
	stateprovider.state('layout2.nav2',{url:'/nav2',templateUrl:'Templates/Widgets/widget2/widget2.html'});
}]);
jwt._arr={'nav1':['layout1/nav1',''],'nav2':['layout2/nav2','']};
