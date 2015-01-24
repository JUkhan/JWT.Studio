namespace('app.controllers.widget1Ctrl', jwt.controllers.baseCtrl.extend({
    scope: null,
    init: function (scope, sce) {
        this._super(scope, sce);
        scope.showMessage = this.showMessage.bind(this);
    },
    showMessage: function (val) {
       alert(val);
       
    }
}))
angular.module('app').controller('widget1Ctrl', ['$scope', '$sce', app.controllers.widget1Ctrl]);