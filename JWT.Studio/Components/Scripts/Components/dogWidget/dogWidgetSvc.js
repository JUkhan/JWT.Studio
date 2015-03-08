class dogWidgetSvc
{
	constructor(){
	}
	static dogWidgetFactory()	{
		return new dogWidgetSvc();
	}
}
export default dogWidgetSvc.dogWidgetFactory;