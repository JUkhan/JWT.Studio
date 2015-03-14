
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/nav1');

	stateprovider.state('root',{abstract:true,url:'/root',templateUrl:'Scripts/Layouts/root/root.html',controller:'rootCtrl as vm'});
	stateprovider.state('root.complex',{abstract:true,url:'/complex',templateUrl:'Scripts/Layouts/complex/complex.html',controller:'complexCtrl as vm'});

	stateprovider.state('root.nav1',{url:'/nav1',templateUrl:'Scripts/Components/widget1/widget1.html',controller:'widget1Ctrl as vm'});
	stateprovider.state('root.nav2',{url:'/nav2',templateUrl:'Scripts/Components/widget2/widget2.html',controller:'widget2Ctrl as vm'});
	stateprovider.state('root.complex.all',{url:'/all',views:{'col1':{templateUrl:'Scripts/Components/widget1/widget1.html',controller:'widget1Ctrl as vm'},'col2':{templateUrl:'Scripts/Components/widget2/widget2.html',controller:'widget2Ctrl as vm'},'col3':{templateUrl:'Scripts/Components/ims/ims.html',controller:'imsCtrl as vm'},'col4':{}}});
	stateprovider.state('root.nav3',{url:'/nav3',templateUrl:'Scripts/Components/ims/ims.html',controller:'imsCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
