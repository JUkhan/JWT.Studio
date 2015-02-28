import BaseCtrl from 'Scripts/Controllers/BaseCtrl.js';

class nav2Ctrl extends BaseCtrl
{
	constructor(scope){
      	super(scope);
		this.title='nav2';
	}
  
 	filterValueChanged(obj){
     this.title=obj.newValue;
    }
}
nav2Ctrl.$inject=['$scope'];

export default nav2Ctrl;