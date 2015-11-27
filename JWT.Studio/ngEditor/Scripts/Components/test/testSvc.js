import BaseSvc from 'Scripts/Base/BaseSvc.js';

class testSvc extends BaseSvc
{
	constructor(http){
		super(http);
		this.http= http;
	}
	static testFactory(http)	{
		return new testSvc(http);
	}
}
testSvc.testFactory.$inject=['$http'];
export default testSvc.testFactory;