
import layout1Ctrl from 'Scripts/Controllers/layout1Ctrl.js';
import widget1Ctrl from 'Scripts/Controllers/widget1Ctrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('layout1Ctrl', layout1Ctrl)
.controller('widget1Ctrl', widget1Ctrl);

export default moduleName;