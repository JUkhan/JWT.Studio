import BaseCtrl from 'Scripts/Base/BaseCtrl.js';

class testCtrl extends BaseCtrl
{
	constructor(scope, svc){
		super(scope);
		this.svc = svc;
		this.title='test';
	}
}
testCtrl.$inject=['$scope', 'testSvc'];
export default testCtrl;