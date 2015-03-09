import mac from 'Scripts/Directives/mac/mac.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('mac', mac.builder)
;

export default moduleName;