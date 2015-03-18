
import chartWidget from 'Scripts/Components/chartWidget/chartWidgetCtrl.js';
import tableWidget from 'Scripts/Components/tableWidget/tableWidgetCtrl.js';
import root from 'Scripts/Layouts/root/rootCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('chartWidgetCtrl', chartWidget)
.controller('tableWidgetCtrl', tableWidget)
.controller('rootCtrl', root);

export default moduleName;