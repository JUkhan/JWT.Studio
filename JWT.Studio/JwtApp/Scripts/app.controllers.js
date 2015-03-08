
import manu2005 from 'Scripts/Components/manu2005/manu2005Ctrl.js';
import dox007 from 'Scripts/Layouts/dox007/dox007Ctrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('manu2005Ctrl', manu2005)
.controller('dox007Ctrl', dox007);

export default moduleName;