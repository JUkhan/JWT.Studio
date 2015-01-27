namespace('app.controllers.widget1Ctrl', jwt.controllers.baseCtrl.extend({
	init: function (scope, sce) {
		this._super(scope, sce);
      	scope.model.msg='Hello world !';
      	scope.name='';
      	//this.initFilter(scope);
	}
}));
angular.module('app').controller('widget1Ctrl', ['$scope', '$sce', app.controllers.widget1Ctrl]);