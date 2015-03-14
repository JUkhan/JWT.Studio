import chart from 'Scripts/Directives/chart/chart.js';
import jwtFilter from 'Scripts/Directives/jwtFilter/jwtFilter.js';
import mac from 'Scripts/Directives/mac/mac.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('chart', chart.builder)
.directive('jwtFilter', jwtFilter.builder)
.directive('mac', mac.builder)
;

export default moduleName;