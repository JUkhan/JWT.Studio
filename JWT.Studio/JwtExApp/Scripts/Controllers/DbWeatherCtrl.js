namespace('app.controllers.DbWeatherCtrl', jwt.controllers.baseCtrl.extend({
	init: function (scope, sce) {
		this._super(scope, sce);
       scope.name='jwt';
      this.initFilter(scope);
	}
}));
angular.module('app').controller('DbWeatherCtrl', ['$scope', '$sce', app.controllers.DbWeatherCtrl]);