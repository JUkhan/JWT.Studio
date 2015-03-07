
import rootCtrl from 'Scripts/Controllers/rootCtrl.js';
import MacWidgetCtrl from 'Scripts/Controllers/MacWidgetCtrl.js';
import jwtFilterWidgetCtrl from 'Scripts/Controllers/jwtFilterWidgetCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('rootCtrl', rootCtrl)
.controller('MacWidgetCtrl', MacWidgetCtrl)
.controller('jwtFilterWidgetCtrl', jwtFilterWidgetCtrl);

export default moduleName;