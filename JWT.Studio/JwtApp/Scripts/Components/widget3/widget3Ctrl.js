import BaseCtrl from 'Scripts/base/BaseCtrl.js';

const SVC=new WeakMap();

class widget3Ctrl extends BaseCtrl
{
	constructor(scope, svc){
		super(scope);
		SVC.set(this, svc);      	
		this.title='widget3';
      	this.getData();
	}
  	getData(){
      let obj={limit:5, columns:[
               {name:'User Id', type:'int'},
          		{name:'User Name', type:'human'},
         		 {name:'Country', type:'country'}
               ]};
    
      
      SVC.get(this).getDummyData(obj) .success(res=>{ this.list=angular.fromJson(res.data); });  
    }
}
widget3Ctrl.$inject=['$scope', 'widget3Svc'];
export default widget3Ctrl;

