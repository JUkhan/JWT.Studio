angular.module("jwt2", ["ui.router", "ngResource", 'ui.bootstrap', 'SignalR','LocalStorageModule'])
.factory('jwtSvc', ['$rootScope', 'Hub', '$timeout', 'localStorageService',  function (rootScope, Hub, $timeout, localStorageService) {
    var jwtSvc = this;
    //Hub setup
    var hub = new Hub('jwt', {
        ROOT_PATH
        listeners: {
            'newConnection': function (name) {
                rootScope.$broadcast("newConnection", name);
            },
            'removeConnection': function (name) {
                rootScope.$broadcast("removeConnection", name);
            },
            'lockFile': function (file) {
                rootScope.$broadcast("lockFile", file);
            },
            'unlockFile': function (file) {
                rootScope.$broadcast("unlockFile", file);
            },
            'receiveMessage': function (data) {
                rootScope.$broadcast("receiveMessage", data);
            },
            'onlineUsers': function (data) {
                rootScope.$broadcast("onlineUsers", data);
            },
        },
        methods: ['lock', 'unlock', 'sendMessage','initHub'],
        errorHandler: function (error) {
            console.error(error);
        },
        hubDisconnected: function () {
            if (hub.connection.lastError) {
                $timeout(function () { hub.connect(); }, 5000);
            }
        }
        //,transport: 'webSockets'
        //,logging: true

    });
    var authData = localStorageService.get('authorizationData');
    if (authData) {	    
        //$.signalR.ajaxDefaults.headers = { Authorization: "Bearer " + authData.token };
        jwtSvc.userName = authData.userName||'unknowen';
    } else {
        jwtSvc.userName = 'unknowen';
    }
    jwtSvc.connectionDone=hub.promise;
    jwtSvc.lock = function (file) {
        try { hub.lock(file); } catch (error) { }
    };
    jwtSvc.unlock = function (file) {
        try { hub.unlock(file); } catch (error) { }
    };
    jwtSvc.sendMessage = function (sender, sendto, message) {
        try { hub.sendMessage(sender, sendto, message); } catch (error) { }
    };
    jwtSvc.initHub = function (user) {
        try { hub.initHub(user); } catch (error) { }
    }
    return jwtSvc;
}])
.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
})
.controller('ModalInstanceCtrl', ['$scope', '$modalInstance', 'data', 'jwtSvc', '$rootScope', function (scope, modalInstance, data, jwtSvc) {

    scope.sendto = data.sendto;
    scope.list = data.list;
    scope.close = function () {
        modalInstance.close();
    };
    scope.send = function (message) {
        if(!message){return;}
        jwtSvc.sendMessage(data.sender, data.sendto, message);
        scope.list.push({ sender: data.sender, message: message });
        scope.message = '';
        scrollTop();
    };
}]);





