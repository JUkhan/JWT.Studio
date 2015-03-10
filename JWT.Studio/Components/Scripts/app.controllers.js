
import chartWidget from 'Scripts/Components/chartWidget/chartWidgetCtrl.js';
import root from 'Scripts/Layouts/root/rootCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('chartWidgetCtrl', chartWidget)
.controller('rootCtrl', root);

export default moduleName;