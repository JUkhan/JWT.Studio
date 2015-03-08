
import Course from 'Scripts/Components/Course/CourseSvc.js';
import Student from 'Scripts/Components/Student/StudentSvc.js';
import Enrollment from 'Scripts/Components/Enrollment/EnrollmentSvc.js';
import newItem from 'Scripts/Components/newItem/newItemSvc.js';

var moduleName='app.services';

angular.module(moduleName,[])
.factory('CourseSvc', Course)
.factory('StudentSvc', Student)
.factory('EnrollmentSvc', Enrollment)
.factory('newItemSvc', newItem);

export default moduleName;