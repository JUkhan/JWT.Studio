class jac
{
	constructor(){
		this.restrict='E';
		this.templateUrl='Scripts/Directives/jac/jac.html';
	}
	static builder()	{
		return new jac();
	}
}
export default jac;