angular.module("jwt2", ["ui.router", "ngResource", 'ui.bootstrap', 'ui.codemirror', 'SignalR'])
.factory('jwtSvc', ['$rootScope', 'Hub', function (rootScope, Hub) {
    var jwtSvc = this;
    //Hub setup
    var hub = new Hub('jwt', {
        ROOT_PATH
        listeners: {
            'newConnection': function (id) {
                rootScope.$broadcast("newConnection", { id:id });
            },
            'removeConnection': function (id) {
                rootScope.$broadcast("removeConnection", { id: id });
            },
            'lockFile': function (file) {
                rootScope.$broadcast("lockFile", file);
            },
            'unlockFile': function (file) {
                rootScope.$broadcast("unlockFile", file);
            }
        },
        methods: ['lock','unlock'],
        errorHandler: function (error) {
            console.error(error);
        }
    });

    jwtSvc.lock = function (file) {
        hub.lock(file);
    };
    jwtSvc.unlock = function (file) {
        hub.unlock(file);
    };
    return jwtSvc;
}]);




