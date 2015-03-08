class MacWidgetSvc
{
	constructor(){
	}
	static macWidgetFactory()	{
		return new MacWidgetSvc();
	}
}
export default MacWidgetSvc.macWidgetFactory;