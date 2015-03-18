import BaseSvc from 'Scripts/base/BaseSvc.js';
const HTTP=new WeakMap();
class tableWidgetSvc extends BaseSvc
{
	constructor(http){
		super(http);
		HTTP.set(this, http);
	}
	static tableWidgetFactory(http)	{
		return new tableWidgetSvc(http);
	}
}
tableWidgetSvc.tableWidgetFactory.$inject=['$http'];
export default tableWidgetSvc.tableWidgetFactory;