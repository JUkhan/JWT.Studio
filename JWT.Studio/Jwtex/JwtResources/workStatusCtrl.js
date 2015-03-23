
angular.module('workStatusApp').controller('workStatusCtrl', ['$scope', 'jwtSvc', function (scope, jwtSvc) {

    scope.list = [];
    //
    scope.load = function () {
        jwtSvc.getWorkStatus();
    };
    scope.$on('workStatus', function (event, list) {
        scope.list = list;
        scope.$apply();
    });

    scope.$on('lockFile', function (event, file) {        
        scope.list.push(file);       
        scope.$apply();
    });

    scope.$on('unlockFile', function (event, file) {       
        scope.list.push(file);
        scope.$apply();
    });
}]);
