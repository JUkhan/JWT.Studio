
import CourseSvc from 'Scripts/Services/CourseSvc.js';
import StudentSvc from 'Scripts/Services/StudentSvc.js';
import EnrollmentSvc from 'Scripts/Services/EnrollmentSvc.js';

var moduleName='app.Services';

angular.module(moduleName,[])
.factory('CourseSvc', CourseSvc.courseFactory)
.factory('StudentSvc', StudentSvc.studentFactory)
.factory('EnrollmentSvc', EnrollmentSvc.enrollmentFactory);

export default moduleName;