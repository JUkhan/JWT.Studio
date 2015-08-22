
export default function config(stateprovider, routeProvider){
	routeProvider.otherwise('root/nav1');

	stateprovider.state('root',{abstract:true,url:'/root',templateUrl:'Scripts/Layouts/root/root.html',controller:'rootCtrl as vm'});
	stateprovider.state('root.complex',{abstract:true,url:'/complex',templateUrl:'Scripts/Layouts/complex/complex.html',controller:'complexCtrl as vm'});

	stateprovider.state('root.nav1',{url:'/nav1',templateUrl:'Scripts/Components/widget1/widget1.html',controller:'widget1Ctrl as vm',resolve:{
	
	test:function(testData){
	    
		return testData.data;
	},
	testData:function($stateParams, widget1Svc){
	    
	   return widget1Svc.getTestData();
	
	
	}
}});
	stateprovider.state('root.complex.all',{url:'/all',views:{'col1':{templateUrl:'Scripts/Components/widget1/widget1.html',controller:'widget1Ctrl as vm',resolve:{
	
	test:function(testData){
	    
		return testData.data;
	},
	testData:function($stateParams, widget1Svc){
	    
	   return widget1Svc.getTestData();
	
	
	}
}},'col2':{templateUrl:'Scripts/Components/widget2/widget2.html',controller:'widget2Ctrl as vm'},'col3':{templateUrl:'Scripts/Components/widget3/widget3.html',controller:'widget3Ctrl as vm'},'col4':{templateUrl:'Scripts/Components/ims/ims.html',controller:'imsCtrl as vm'}}});
	stateprovider.state('root.nav2',{url:'/nav2',templateUrl:'Scripts/Components/ims/ims.html',controller:'imsCtrl as vm'});
	stateprovider.state('root.nav3',{url:'/nav3',templateUrl:'Scripts/Components/test/test.html',controller:'testCtrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
