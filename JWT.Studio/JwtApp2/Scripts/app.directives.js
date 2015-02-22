

import JwtFilter from 'Scripts/Directives/JwtFilter.js';

var moduleName='app.Directives';

angular.module(moduleName,[])
.factory('jwtFilter', JwtFilter.builder);

export default moduleName;


