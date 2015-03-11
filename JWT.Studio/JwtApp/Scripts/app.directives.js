import chart from 'Scripts/Directives/chart/chart.js';
import mac from 'Scripts/Directives/mac/mac.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('chart', chart.builder)
.directive('mac', mac.builder)
;

export default moduleName;