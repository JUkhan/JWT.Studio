
import widget1 from 'Scripts/Components/widget1/widget1Ctrl.js';
import widget2 from 'Scripts/Components/widget2/widget2Ctrl.js';
import ims from 'Scripts/Components/ims/imsCtrl.js';
import root from 'Scripts/Layouts/root/rootCtrl.js';
import complex from 'Scripts/Layouts/complex/complexCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('widget1Ctrl', widget1)
.controller('widget2Ctrl', widget2)
.controller('imsCtrl', ims)
.controller('rootCtrl', root)
.controller('complexCtrl', complex);

export default moduleName;