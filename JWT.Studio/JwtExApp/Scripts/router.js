angular.module('app').config(['$stateProvider', '$urlRouterProvider', function (stateprovider, routeProvider) {


	stateprovider.state('nav1',{url:'/nav1',templateUrl:'Templates/Widgets/DbWeather.html',controller:'DbWeatherCtrl'});
}]);
jwt._arr={'nav1':['nav1','']};
