
import widget1 from 'Scripts/Components/widget1/widget1Svc.js';
import widget2 from 'Scripts/Components/widget2/widget2Svc.js';
import ims from 'Scripts/Components/ims/imsSvc.js';

var moduleName='app.services';

angular.module(moduleName,[])
.factory('widget1Svc', widget1)
.factory('widget2Svc', widget2)
.factory('imsSvc', ims);

export default moduleName;