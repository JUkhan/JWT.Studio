class imsSvc
{
	constructor(){
	}
	static imsFactory()	{
		return new imsSvc();
	}
}
export default imsSvc.imsFactory;