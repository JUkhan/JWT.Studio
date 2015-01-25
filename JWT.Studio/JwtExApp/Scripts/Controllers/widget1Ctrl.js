namespace('app.controllers.widget1Ctrl', jwt.controllers.baseCtrl.extend({
	init: function (scope, sce) {
		this._super(scope, sce);
      	scope.model.msg='Hello world !';
	}
}));
angular.module('app').controller('widget1Ctrl', ['$scope', '$sce', app.controllers.widget1Ctrl]);