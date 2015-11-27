
import test from 'Scripts/Components/test/testCtrl.js';
import root from 'Scripts/Layouts/root/rootCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('testCtrl', test)
.controller('rootCtrl', root);

export default moduleName;