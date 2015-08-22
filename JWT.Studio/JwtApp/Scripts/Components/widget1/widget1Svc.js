import BaseSvc from 'Scripts/base/BaseSvc.js';

const HTTP=new WeakMap();
  
class widget1Svc extends BaseSvc
{
	constructor(http){
      super(http);
      HTTP.set(this, http);
	}
	getTestData(){
	      var data_config={
  	     limit:10,
  	     columns:[{field:'age', type:'int', min:20, max:35},{field:'name', type:'human'},
  	     {field:'price', type:'double', min:50000, max:500000},{field:'selling', type:'int', array:true, limit:15, min:1, max:15} ]  
  	   };
  	   
  	   return this.getDummyData(data_config);
	}
	static widget1Factory(http)	{
		return new widget1Svc(http);
	}
}
widget1Svc.widget1Factory.$inject=['$http'];

export default widget1Svc.widget1Factory;