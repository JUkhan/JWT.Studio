import BaseCtrl from 'Scripts/Controllers/BaseCtrl.js';

class nav1Ctrl extends BaseCtrl
{
	constructor(scope){
      	super(scope);
		this.title='nav1';
      	this.msg='Hello world';
      	this.initFilter();
	}
}

nav1Ctrl.$inject=['$scope'];

export default nav1Ctrl;