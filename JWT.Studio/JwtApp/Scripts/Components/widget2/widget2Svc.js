import BaseSvc from 'Scripts/base/BaseSvc.js';

const HTTP=new WeakMap();
  
class widget2Svc extends BaseSvc
{
	constructor(http){
      super(http);
      HTTP.set(this, http);
	}
	static widget2Factory(http)	{
		return new widget2Svc(http);
	}
}
widget2Svc.widget2Factory.$inject=['$http'];

export default widget2Svc.widget2Factory;