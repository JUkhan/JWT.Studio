
import chartWidget from 'Scripts/Components/chartWidget/chartWidgetSvc.js';
import tableWidget from 'Scripts/Components/tableWidget/tableWidgetSvc.js';

var moduleName='app.services';

angular.module(moduleName,[])
.factory('chartWidgetSvc', chartWidget)
.factory('tableWidgetSvc', tableWidget);

export default moduleName;