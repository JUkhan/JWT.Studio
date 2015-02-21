
import nav1Ctrl from 'Scripts/Controllers/nav1Ctrl.js';
import nav2Ctrl from 'Scripts/Controllers/nav2Ctrl.js';
import CourseCtrl from 'Scripts/Controllers/CourseCtrl.js';
import StudentCtrl from 'Scripts/Controllers/StudentCtrl.js';
import EnrollmentCtrl from 'Scripts/Controllers/EnrollmentCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('nav1Ctrl', nav1Ctrl)
.controller('nav2Ctrl', nav2Ctrl)
.controller('CourseCtrl', CourseCtrl)
.controller('StudentCtrl', StudentCtrl)
.controller('EnrollmentCtrl', EnrollmentCtrl);

export default moduleName;