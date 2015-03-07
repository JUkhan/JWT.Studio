
import CourseSvc from 'Scripts/Services/CourseSvc.js';
import StudentSvc from 'Scripts/Services/StudentSvc.js';
import EnrollmentSvc from 'Scripts/Services/EnrollmentSvc.js';

var moduleName='app.services';

angular.module(moduleName,[])
.factory('CourseSvc', CourseSvc)
.factory('StudentSvc', StudentSvc)
.factory('EnrollmentSvc', EnrollmentSvc);

export default moduleName;