
import BaseCtrl from 'Scripts/base/BaseCtrl.js';

class widget2Ctrl extends BaseCtrl
{
	constructor(scope){
      	super(scope);
		this.title='widget2';
	}
  
  	filterValueChanged(obj){
      console.log(obj);
    }
}

widget2Ctrl.$inject=['$scope'];
export default widget2Ctrl;