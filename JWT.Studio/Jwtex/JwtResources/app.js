angular.module("jwt2", ["ui.router", "ngResource", 'ui.bootstrap', 'ui.codemirror', 'SignalR'])
.factory('jwtSvc', ['$rootScope', 'Hub', function (rootScope, Hub) {
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
        methods: ['lock', 'unlock', 'sendMessage'],
        errorHandler: function (error) {
            console.error(error);
        }
    });

    jwtSvc.lock = function (file) {
        try{ hub.lock(file);}catch(error){}
    };
    jwtSvc.unlock = function (file) {
        try{hub.unlock(file);}catch(error){}
    };
    jwtSvc.sendMessage = function (user, message) {
        try{hub.sendMessage(user, message);}catch(error){}
    };
    return jwtSvc;
}])
.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if(event.which === 13) {
                scope.$apply(function (){
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
        jwtSvc.sendMessage(data.sendto, message);
        scope.list.push({ sender: data.sender, message: message });
        scope.message = '';
        scrollTop();
    };
}]);





