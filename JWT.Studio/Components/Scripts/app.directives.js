import comInstaller from 'Scripts/Directives/comInstaller/comInstaller.js';
import jwtFilter from 'Scripts/Directives/jwtFilter/jwtFilter.js';
import mac from 'Scripts/Directives/mac/mac.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('comInstaller', comInstaller.builder)
.directive('jwtFilter', jwtFilter.builder)
.directive('mac', mac.builder)
;

export default moduleName;