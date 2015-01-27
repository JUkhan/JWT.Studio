namespace('app.controllers.widget2Ctrl', jwt.controllers.baseCtrl.extend({
  	scope:null,
	init: function (scope, sce) {
		this._super(scope, sce);
      	this.scope=scope;
	}
  /*,
  	onFilterValueChange:function(e, fname, newVal, oldVal){
  		this.scope.msg=newVal;
	}*/
}));
angular.module('app').controller('widget2Ctrl', ['$scope', '$sce', app.controllers.widget2Ctrl]);