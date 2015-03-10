
import chartWidget from 'Scripts/Components/chartWidget/chartWidgetSvc.js';

var moduleName='app.services';

angular.module(moduleName,[])
.factory('chartWidgetSvc', chartWidget);

export default moduleName;