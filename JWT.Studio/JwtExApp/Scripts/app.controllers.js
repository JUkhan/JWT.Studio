
import nav3LayoutCtrl from 'Scripts/Controllers/nav3LayoutCtrl.js';
import widget007Ctrl from 'Scripts/Controllers/widget007Ctrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('nav3LayoutCtrl', nav3LayoutCtrl)
.controller('widget007Ctrl', widget007Ctrl);

export default moduleName;