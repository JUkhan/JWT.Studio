
import widget1 from 'Scripts/Components/widget1/widget1Svc.js';
import widget2 from 'Scripts/Components/widget2/widget2Svc.js';
import widget3 from 'Scripts/Components/widget3/widget3Svc.js';
import ims from 'Scripts/Components/ims/imsSvc.js';
import test from 'Scripts/Components/test/testSvc.js';

var moduleName='app.services';

angular.module(moduleName,[])
.factory('widget1Svc', widget1)
.factory('widget2Svc', widget2)
.factory('widget3Svc', widget3)
.factory('imsSvc', ims)
.factory('testSvc', test);

export default moduleName;