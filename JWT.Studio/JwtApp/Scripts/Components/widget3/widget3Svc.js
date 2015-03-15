import BaseSvc from 'Scripts/base/BaseSvc.js';

const HTTP=new WeakMap();

class widget3Svc extends BaseSvc
{
	constructor(http){
		super(http);
		HTTP.set(this, http);
	}
	static widget3Factory(http)	{
		return new widget3Svc(http);
	}
}

widget3Svc.widget3Factory.$inject=['$http'];

export default widget3Svc.widget3Factory;

