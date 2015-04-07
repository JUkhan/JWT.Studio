
import BaseCtrl from 'Scripts/base/BaseCtrl.js';

const SVC=new WeakMap();

class widget2Ctrl extends BaseCtrl
{
	constructor(scope, svc){
      	super(scope);
		this.title='widget2';
      	SVC.set(this, svc);    	
        
	}
  
  	filterValueChanged(obj){
      this.country=obj.newValue;
      this.loadData();
    }
  	
  	loadData(){
      
      let obj={limit:Math.floor(Math.random()*11)+1, columns:[
        {name:'id', type:'int'},
        {name:'name', type:'animal'}
      ]};
     
      SVC.get(this).getDummyData(obj)
      .success(res=>{ this.list=angular.fromJson(res.data); });  
   
      
    }
  	
}

widget2Ctrl.$inject=['$scope', 'widget2Svc'];
export default widget2Ctrl;