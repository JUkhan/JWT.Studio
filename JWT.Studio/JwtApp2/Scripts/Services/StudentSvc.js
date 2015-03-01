import BaseSvc from 'Scripts/Services/BaseSvc.js';

const HTTP=new WeakMap();
class StudentSvc extends BaseSvc
{
	constructor(http)
	{
		super('Student',http);
		HTTP.set(this, http);
	}
	static studentFactory(http)
	{
		return new StudentSvc(http);
	}
}
StudentSvc.studentFactory.$inject=['$http'];
export default StudentSvc.studentFactory;