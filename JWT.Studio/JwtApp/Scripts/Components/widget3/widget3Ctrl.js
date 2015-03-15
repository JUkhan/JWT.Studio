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
               {name:'userId', type:'int'},
          		{name:'userName', type:'human'},
         		 {name:'country', type:'country'}
               ]};
      let me=this;
      
      SVC.get(this).getDummyData(obj) .success(res=>{ me.list=angular.fromJson(res.data); });  
    }
}
widget3Ctrl.$inject=['$scope', 'widget3Svc'];
export default widget3Ctrl;

