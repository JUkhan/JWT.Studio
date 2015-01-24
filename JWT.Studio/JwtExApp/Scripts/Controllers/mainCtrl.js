namespace('app.controllers.mainCtrl', jwt.controllers.baseCtrl.extend({
    scope: null,
    init: function (scope, sce) {
        this._super(scope, sce);
        scope.url = this.url.bind(this);
        scope.msg = ' well-come jwt';

    },
    url: function (navName, paramValue) {
        return jwt.url(navName, paramValue);
    }
}))
angular.module('app').controller('mainCtrl', ['$scope', '$sce', app.controllers.mainCtrl]);