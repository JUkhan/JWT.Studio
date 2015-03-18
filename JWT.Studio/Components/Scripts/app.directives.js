import chart from 'Scripts/Directives/chart/chart.js';
import comInstaller from 'Scripts/Directives/comInstaller/comInstaller.js';
import tableCom from 'Scripts/Directives/tableCom/tableCom.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('chart', chart.builder)
.directive('comInstaller', comInstaller.builder)
.directive('tableCom', tableCom.builder)
;

export default moduleName;