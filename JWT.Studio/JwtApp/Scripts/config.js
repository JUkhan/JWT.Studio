
export default function config(stateprovider, routeProvider){

	stateprovider.state('Course',{url:'/Course',templateUrl:'Scripts/Components/Course202/Course202.html',controller:'Course202Ctrl as vm'});
}
config.$inject=['$stateProvider', '$urlRouterProvider'];
