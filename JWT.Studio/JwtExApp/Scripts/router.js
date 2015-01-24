angular.module('app').config(['$stateProvider', '$urlRouterProvider', function (stateprovider, routeProvider) {

	stateprovider.state('layout1',{abstract:true,url:'/layout1',templateUrl:'Templates/Layouts/layout1.html'});

	stateprovider.state('layout1.nav1',{url:'/nav1',views:{'col1':{templateUrl:'Templates/Widgets/widget1/widget1.html',controller:'widget1Ctrl'},'col2':{templateUrl:'Templates/Widgets/widget1/widget1.html',controller:'widget1Ctrl'}}});
}]);
jwt._arr={'nav1':['layout1/nav1','']};
