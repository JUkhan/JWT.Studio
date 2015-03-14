import BaseSvc from 'Scripts/base/BaseSvc.js';
  
class widget2Svc extends BaseSvc
{
	constructor(http){
      super(http);
	}
	static widget2Factory(http)	{
		return new widget2Svc(http);
	}
}
widget2Svc.widget2Factory.$inject=['$http'];

export default widget2Svc.widget2Factory;