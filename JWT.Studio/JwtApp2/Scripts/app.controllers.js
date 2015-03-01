
import rootLayoutCtrl from 'Scripts/Controllers/rootLayoutCtrl.js';
import imsLayoutCtrl from 'Scripts/Controllers/imsLayoutCtrl.js';
import CourseCtrl from 'Scripts/Controllers/CourseCtrl.js';
import StudentCtrl from 'Scripts/Controllers/StudentCtrl.js';
import EnrollmentCtrl from 'Scripts/Controllers/EnrollmentCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('rootLayoutCtrl', rootLayoutCtrl)
.controller('imsLayoutCtrl', imsLayoutCtrl)
.controller('CourseCtrl', CourseCtrl)
.controller('StudentCtrl', StudentCtrl)
.controller('EnrollmentCtrl', EnrollmentCtrl);

export default moduleName;