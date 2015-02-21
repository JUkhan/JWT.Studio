
import DbWeatherCtrl from 'Scripts/Controllers/DbWeatherCtrl.js';
import testWidgetCtrl from 'Scripts/Controllers/testWidgetCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('DbWeatherCtrl', DbWeatherCtrl)
.controller('testWidgetCtrl', testWidgetCtrl);

export default moduleName;