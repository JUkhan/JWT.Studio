import BaseSvc from 'Scripts/base/BaseSvc.js';
const HTTP=new WeakMap();
class homeSvc extends BaseSvc
{
	constructor(http){
		super(http);
		HTTP.set(this, http);
	}
	static homeFactory(http)	{
		return new homeSvc(http);
	}
}
homeSvc.homeFactory.$inject=['$http'];
export default homeSvc.homeFactory;