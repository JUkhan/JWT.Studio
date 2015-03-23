
import home from 'Scripts/Components/home/homeCtrl.js';
import root from 'Scripts/Layouts/root/rootCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('homeCtrl', home)
.controller('rootCtrl', root);

export default moduleName;