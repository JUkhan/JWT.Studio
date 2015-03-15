import BaseCtrl from 'Scripts/base/BaseCtrl.js';

const SVC=new WeakMap();

class widget1Ctrl extends BaseCtrl
{
	constructor(scope, svc){
      super(scope);
	  SVC.set(this, svc);	
      this.title='widget1';
      this.country='';      	
      this.initFilter();
      this.loadData();
	}
  	loadData(){
      
      this.countryList=["Bangladesh", "India", "Japan", "China"];
   
    }
}
widget1Ctrl.$inject=['$scope', 'widget1Svc'];
export default widget1Ctrl;