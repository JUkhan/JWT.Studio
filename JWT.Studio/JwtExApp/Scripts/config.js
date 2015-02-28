
export default function config(stateprovider, routeProvider){
	stateprovider.state('nav3Layout',{abstract:true,url:'/nav3Layout',templateUrl:'Templates/Layouts/nav3Layout.html',controller:'nav3LayoutCtrl'});
	stateprovider.state('layout2',{abstract:true,url:'/layout2',templateUrl:'Templates/Layouts/layout2.html'});

	stateprovider.state('nav1',{url:'/nav1',templateUrl:'Templates/Widgets/widget007.html',controller:'widget007Ctrl as vm'});
	stateprovider.state('nav3Layout.nav2',{url:'/nav2'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
