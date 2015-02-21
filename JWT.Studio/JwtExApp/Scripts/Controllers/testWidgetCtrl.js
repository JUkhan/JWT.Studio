
import BaseCtrl from 'Scripts/Controllers/BaseCtrl.js';

let httpService=new WeakMap();
class testWidgetCtrl extends BaseCtrl
{
	constructor(http)
  	{
		this.title='testWidget ....101';
      	httpService.set(this, http);
      	      
	}
  	loadData()
  	{       
      let that=this;
      this.async(function*(){        
        	let data1=yield httpService.get(that).get('test/data1');
        	let data2=yield httpService.get(that).get('test/data2/'+data1.id);
        	that.msg=data1.msg+data2.msg;        	
      });
    }
  	
  	static instance(http)
  	{
      	return new testWidgetCtrl(http);
    }
}
testWidgetCtrl.instance.$inject=['$http'];
export default testWidgetCtrl.instance;