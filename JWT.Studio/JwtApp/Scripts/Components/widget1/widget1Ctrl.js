import BaseCtrl from 'Scripts/base/BaseCtrl.js';

class widget1Ctrl extends BaseCtrl
{
	constructor(scope){
      	super(scope);
      	
      this.title='widget1';
		this.country='';
      	this.countryList=['Bangladesh', 'India', 'Pakistan'];
       this.initFilter();
      console.log(this.country);
	}
}
widget1Ctrl.$inject=['$scope'];
export default widget1Ctrl;