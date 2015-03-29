
import config from 'Scripts/config.js';
import authInterceptorService from 'Scripts/Base/authInterceptorService.js';
import authService from 'Scripts/Base/authService.js';
import {default as controllers} from 'Scripts/app.controllers.js';
import {default as services} from 'Scripts/app.services.js';
import {default as directives} from 'Scripts/app.directives.js';
import {default as filters} from 'Scripts/app.filters.js';


var moduleName='app'; 

angular.module(moduleName,['ui.router', 'ngResource', 'LocalStorageModule', 'angular-loading-bar', controllers, services, directives, filters])
    .factory('authInterceptorService', authInterceptorService)
    .factory('authService', authService)
    .config(config)
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('authInterceptorService');
    })
    .constant('ngAuthSettings', {
        stsServiceBaseUri: 'http://localhost:21545/',        
        apiServiceBaseUri: 'http://localhost:22752/',
        clientId: 'nativeApp'//nativeApp//jwtApp
    })
    .run(['authService', '$rootScope', '$templateCache', function(authService, $rootScope, $templateCache) {
        authService.fillAuthData();
        $rootScope.$on('$viewContentLoaded', function() {
            $templateCache.removeAll();
        });
    }]);

export default moduleName;

