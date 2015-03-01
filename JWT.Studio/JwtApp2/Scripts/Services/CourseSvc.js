import BaseSvc from 'Scripts/Services/BaseSvc.js';

const HTTP=new WeakMap();
class CourseSvc extends BaseSvc
{
	constructor(http)
	{
		super('Course',http);
		HTTP.set(this, http);
	}
	static courseFactory(http)
	{
		return new CourseSvc(http);
	}
}
CourseSvc.courseFactory.$inject=['$http'];
export default CourseSvc.courseFactory;