import chart from 'Scripts/Directives/chart/chart.js';
import comInstaller from 'Scripts/Directives/comInstaller/comInstaller.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('chart', chart.builder)
.directive('comInstaller', comInstaller.builder)
;

export default moduleName;