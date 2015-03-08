
export default function config(stateprovider, routeProvider){
	stateprovider.state('dox007',{abstract:true,url:'/dox007',templateUrl:'Scripts/Layouts/dox007/dox007.html',controller:'dox007Ctrl as vm'});

	stateprovider.state('valu',{url:'/valu',templateUrl:'Scripts/Components/manu2005/manu2005.html',controller:'manu2005Ctrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
