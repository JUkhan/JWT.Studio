import BaseCtrl from 'Scripts/base/BaseCtrl.js';

const SVC=new WeakMap();

class widget1Ctrl extends BaseCtrl
{
    constructor(scope, svc, testData, test){
      super(scope);
	  SVC.set(this, svc);	
      this.title='widget1';
      this.country='';      	
      this.initFilter();
      this.loadData();
      console.log(testData, test);
	}
  	loadData(){
      
      this.countryList=["Bangladesh", "India", "Japan", "China"];
   
    }
}
widget1Ctrl.$inject=['$scope', 'widget1Svc','testData','test'];
export default widget1Ctrl;