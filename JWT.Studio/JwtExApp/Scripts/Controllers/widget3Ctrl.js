namespace('app.controllers.widget3Ctrl', jwt.controllers.baseCtrl.extend({
	init: function (scope, sce) {
		this._super(scope, sce);
	}
}));
angular.module('app').controller('widget3Ctrl', ['$scope', '$sce', app.controllers.widget3Ctrl]);