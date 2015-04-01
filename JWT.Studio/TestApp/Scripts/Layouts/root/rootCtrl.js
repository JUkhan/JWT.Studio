
class rootCtrl
{
	constructor(location, authService){
      
		this.authentication = authService.authentication;
      	this.authService=authService;
      	this.location=location;
	}
  	logOut() {
        this.authService.logOut();
        this.location.path('root/login');
    }
  	
}
rootCtrl.$inject=['$location', 'authService'];
export default rootCtrl;