class masSvc
{
	constructor(){
	}
	static masFactory()	{
		return new masSvc();
	}
}
export default masSvc.masFactory;