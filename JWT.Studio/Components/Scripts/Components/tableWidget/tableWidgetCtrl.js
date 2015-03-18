import BaseCtrl from 'Scripts/base/BaseCtrl.js';
const SVC=new WeakMap();
class tableWidgetCtrl extends BaseCtrl
{
	constructor(scope, svc){
		super(scope);
		SVC.set(this, svc);
		this.name='tableCom';
      	this.description='Table component renders list of data';
     
      	this.loadData();
	}
  	
  	loadData(){
      
      let dataConfig={limit:20, columns:[
        {name:'Country', type:'country'},
        {name:'Name', type:'human'},
        {name:'Age', type:'int', min:20, max:100},
        {name:'IsMarried', type:'bool'},
        {name:'Beloved', type:'animal'}
      ]};      
      
      SVC.get(this).getDummyData(dataConfig)
        .success(res=>{ this.list=angular.fromJson(res.data); });
    }
}
tableWidgetCtrl.$inject=['$scope', 'tableWidgetSvc'];
export default tableWidgetCtrl;