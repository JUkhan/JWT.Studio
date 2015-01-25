namespace('app.controllers.widget2Ctrl', jwt.controllers.baseCtrl.extend({
	init: function (scope, sce) {
		this._super(scope, sce);
	}
}));
angular.module('app').controller('widget2Ctrl', ['$scope', '$sce', app.controllers.widget2Ctrl]);