import BaseSvc from 'Scripts/Services/BaseSvc.js';

const HTTP=new WeakMap();
class EnrollmentSvc extends BaseSvc
{
	constructor(http)
	{
		super('Enrollment',http);
		HTTP.set(this, http);
	}
	getCourseList()
	{
		return HTTP.get(this).get(this.root() + this.controllerName+"/GetCourseList");
	}
	getStudentList()
	{
		return HTTP.get(this).get(this.root() + this.controllerName+"/GetStudentList");
	}
	static enrollmentFactory(http)
	{
		return new EnrollmentSvc(http);
	}
}
EnrollmentSvc.enrollmentFactory.$inject=['$http'];
export default EnrollmentSvc.enrollmentFactory;