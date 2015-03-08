class dog
{
	constructor(){
		this.restrict='E';
		this.templateUrl='Scripts/Directives/dog/dog.html';
	}
	static builder()	{
		return new dog();
	}
}
export default dog;