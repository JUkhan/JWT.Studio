
import config from 'Scripts/config.js';
import {default as controllers} from 'Scripts/app.controllers.js';
//import {default as services} from 'Scripts/app.services.js';
import {default as directives} from 'Scripts/app.directives.js';
import {default as filters} from 'Scripts/app.filters.js';

var moduleName='app'; 

angular.module(moduleName,['ui.router', controllers, directives, filters]).config(config);
/*.run(function($rootScope, $templateCache){

    $rootScope.$on('$routeChangeStart',function(event, next, current){
        if(typeof(current) !=='undefined'){
            $templateCache.remove(current.templateUrl);
        }
    });

    $rootScope.$on('$viewContentLoded', function(){
        $templateCache.removeAll();
    })
    
})*/

export default moduleName;
