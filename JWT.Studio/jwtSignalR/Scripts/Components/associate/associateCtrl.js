
class associateCtrl 
{
	constructor(location, timeout, authService){
        this.location=location;
      	this.timeout=timeout;
      	this.authService=authService;
      
      	this.savedSuccessfully = false;
    	this.message = "";
      	this.registerData = {
            userName: authService.externalAuthData.userName,
            provider: authService.externalAuthData.provider,
            externalAccessToken: authService.externalAuthData.externalAccessToken
      	};
	}
  	
  	 registerExternal() {
       
        let that=this;
        that.authService.registerExternal(that.registerData).then(function (response) {

            that.savedSuccessfully = true;
            that.message = "User has been registered successfully, you will be redicted to home page in 2 seconds.";
            that.startTimer();

        },
          function (response) {
              var errors = [];
              for (var key in response.modelState) {
                  errors.push(response.modelState[key]);
              }
              that.message = "Failed to register user due to:" + errors.join(' ');
          });
    }
  
  	startTimer() {
      	let that=this;
        var timer = that.timeout(function () {
            that.timeout.cancel(timer);
            that.location.path('root/home');
        }, 2000);
    }
}
associateCtrl.$inject=['$location','$timeout','authService'];
export default associateCtrl;