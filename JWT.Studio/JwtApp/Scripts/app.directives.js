import chart from 'Scripts/Directives/chart/chart.js';
import jwtFilter from 'Scripts/Directives/jwtFilter/jwtFilter.js';
import mac from 'Scripts/Directives/mac/mac.js';
import tableCom from 'Scripts/Directives/tableCom/tableCom.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('chart', chart.builder)
.directive('jwtFilter', jwtFilter.builder)
.directive('mac', mac.builder)
.directive('tableCom', tableCom.builder)
;

export default moduleName;