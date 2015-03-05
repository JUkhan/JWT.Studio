
import jwtDate from 'Scripts/Filters/JwtDate.js';

var moduleName='app.Filters';

angular.module(moduleName,[])
.filter('jwtDate', jwtDate);

export default moduleName;
