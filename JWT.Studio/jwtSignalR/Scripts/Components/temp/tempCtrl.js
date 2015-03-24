import BaseCtrl from 'Scripts/base/BaseCtrl.js';
const SVC=new WeakMap();
class tempCtrl extends BaseCtrl
{
	constructor(scope, svc){
		super(scope);
		SVC.set(this, svc);
		this.title='temp';
	}
}
tempCtrl.$inject=['$scope', 'tempSvc'];
export default tempCtrl;