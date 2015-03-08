import comInstaller from 'Scripts/Directives/comInstaller/comInstaller.js';
import dog from 'Scripts/Directives/dog/dog.js';
import jwtFilter from 'Scripts/Directives/jwtFilter/jwtFilter.js';
import mac from 'Scripts/Directives/mac/mac.js';


var moduleName='app.Directives';

angular.module(moduleName, [])
.directive('comInstaller', comInstaller.builder)
.directive('dog', dog.builder)
.directive('jwtFilter', jwtFilter.builder)
.directive('mac', mac.builder)
;

export default moduleName;