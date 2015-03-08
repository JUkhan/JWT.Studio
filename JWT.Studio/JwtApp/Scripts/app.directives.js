import dog from 'Scripts/Directives/dog/dog.js';
import jac from 'Scripts/Directives/jac/jac.js';
import jwtFilter from 'Scripts/Directives/jwtFilter/jwtFilter.js';
import mac from 'Scripts/Directives/mac/mac.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('dog', dog.builder)
.directive('jac', jac.builder)
.directive('jwtFilter', jwtFilter.builder)
.directive('mac', mac.builder)
;

export default moduleName;