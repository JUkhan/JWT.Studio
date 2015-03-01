
export default function config(stateprovider, routeProvider){
	stateprovider.state('layout1',{abstract:true,url:'/layout1',templateUrl:'Templates/Layouts/layout1.html',controller:'layout1Ctrl as vm'});

	stateprovider.state('nav1',{url:'/nav1',templateUrl:'Templates/Widgets/widget1.html',controller:'widget1Ctrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
