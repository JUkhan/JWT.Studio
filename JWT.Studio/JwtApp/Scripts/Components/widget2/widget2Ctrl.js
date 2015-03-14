
import BaseCtrl from 'Scripts/base/BaseCtrl.js';

const SVC=new WeakMap();

class widget2Ctrl extends BaseCtrl
{
	constructor(scope, svc){
      	super(scope);
		this.title='widget2';
      	SVC.set(this, svc);
      	this.list=[{id:101, name:'Bangladesh'}];
      	
	}
  
  	filterValueChanged(obj){
      this.loadData();
    }
  	
  	loadData(){
      let obj={limit:5, columns:[
        {name:'id', type:'int'},
        {name:'name', type:'country'}
      ]};
      
      SVC.get(this).getDummyData(obj)
      .then(data=>{ this.list=data; });
      
    }
}

widget2Ctrl.$inject=['$scope', 'widget2Svc'];
export default widget2Ctrl;