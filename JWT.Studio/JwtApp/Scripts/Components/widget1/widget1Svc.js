import BaseSvc from 'Scripts/base/BaseSvc.js';

const HTTP=new WeakMap();
  
class widget1Svc extends BaseSvc
{
	constructor(http){
      super(http);
      HTTP.set(this, http);
	}
	static widget1Factory(http)	{
		return new widget1Svc(http);
	}
}
widget1Svc.widget1Factory.$inject=['$http'];

export default widget1Svc.widget1Factory;