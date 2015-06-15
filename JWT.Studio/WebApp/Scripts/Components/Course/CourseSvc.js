import BaseEntitySvc from 'Scripts/base/BaseEntitySvc.js';

const HTTP=new WeakMap();
class CourseSvc extends BaseEntitySvc
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