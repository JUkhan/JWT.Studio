﻿angular.module("workStatusApp", ['ui.router', 'SignalR'])
.factory('jwtSvc', ['$rootScope', 'Hub', function (rootScope, Hub) {
    var jwtSvc = this;
    //Hub setup
    var hub = new Hub('jwt', {
        ROOT_PATH
        listeners: {

            'lockFile': function (file) {
                rootScope.$broadcast("lockFile", file);
            },
            'unlockFile': function (file) {
                rootScope.$broadcast("unlockFile", file);
            },
            'workStatus': function (list) {
                rootScope.$broadcast("workStatus", list);
            }
        },
        methods: ['getWorkStatus'],
        errorHandler: function (error) {
            console.error(error);
        }
    });
    jwtSvc.connectionDone=hub.promise;
    jwtSvc.getWorkStatus = function () {
        hub.getWorkStatus();
    };
   
    return jwtSvc;
}]);




