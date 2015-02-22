

import JwtFilter from 'Scripts/Directives/JwtFilter.js';

var moduleName='app.Directives';

angular.module(moduleName,[])
.directive('jwtFilter', JwtFilter.builder);

export default moduleName;


