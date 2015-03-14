class testSvc
{
	constructor(){
	}
	static testFactory()	{
		return new testSvc();
	}
}
export default testSvc.testFactory;