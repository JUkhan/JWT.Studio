angular.module("app", ["ui.router", "ngSanitize", "ngResource", 'ui.grid', 'ui.grid.paging', 'ui.bootstrap']);
angular.module('app').filter('jwtDate', function () {
    return function (input) {
        var len = 0;
        if (input && (len = input.length) > 8) {
            return input.substring(6, input.length - 2);
        }
        return input;
    };
});
 angular.module('app').directive('jwtFilter', ['$rootScope', function (rootScope) {
	function setVal(prop, val) {
		var ob = window.sessionStorage.getItem("dbFilter") || "{}",
		data = angular.fromJson(ob);
		data[prop] = val;
		window.sessionStorage.setItem("dbFilter", angular.toJson(data));
	}
	return{
		restrict:'A',
		link:function(scope, jquery, attrs, ctrl){
			var filterNaame = attrs.dbFilter || attrs.ngModel;
			scope.$watch(filterNaame, function (newVal, oldVal) {
					rootScope.$broadcast("FilterValueChange", { name: filterNaame, newValue: newVal, oldValue: oldVal });
					setVal(filterNaame, newVal);
			});
		}
	}        
 }]);



