import BaseSvc from 'Scripts/base/BaseSvc.js';
const HTTP=new WeakMap();
class tempSvc extends BaseSvc
{
	constructor(http){
		super(http);
		HTTP.set(this, http);
	}
	static tempFactory(http)	{
		return new tempSvc(http);
	}
}
tempSvc.tempFactory.$inject=['$http'];
export default tempSvc.tempFactory;