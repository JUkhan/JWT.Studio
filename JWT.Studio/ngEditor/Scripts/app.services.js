
import test from 'Scripts/Components/test/testSvc.js';

var moduleName='app.services';

angular.module(moduleName,[])
.factory('testSvc', test);

export default moduleName;