
angular.module('app').controller('mainController', ['$scope', '$http', '$modal', function (scope, http, modal) {
   
    scope.msg = 'Main controller message...';
    scope.editorContent = "angular.module('app').controller('mainCtrl', ['$scope', '$sce', app.controllers.mainCtrl]);";
}]);

