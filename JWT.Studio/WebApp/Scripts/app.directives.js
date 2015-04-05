import jwtFilter from 'Scripts/Directives/jwtFilter/jwtFilter.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('jwtFilter', jwtFilter.builder)
;

export default moduleName;