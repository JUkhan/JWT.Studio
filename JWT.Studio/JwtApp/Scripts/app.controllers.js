
import Course from 'Scripts/Components/Course/CourseCtrl.js';
import Student from 'Scripts/Components/Student/StudentCtrl.js';
import Enrollment from 'Scripts/Components/Enrollment/EnrollmentCtrl.js';
import rootLayout from 'Scripts/Layouts/rootLayout/rootLayoutCtrl.js';
import imsLayout from 'Scripts/Layouts/imsLayout/imsLayoutCtrl.js';

var moduleName='app.controllers';

angular.module(moduleName,[])
.controller('CourseCtrl', Course)
.controller('StudentCtrl', Student)
.controller('EnrollmentCtrl', Enrollment)
.controller('rootLayoutCtrl', rootLayout)
.controller('imsLayoutCtrl', imsLayout);

export default moduleName;