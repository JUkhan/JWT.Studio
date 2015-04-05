
class signupCtrl 
{
	constructor(location, timeout, authService){
		this.location=location;
      	this.timeout=timeout;
       	this.authService=authService;
      
      	this.savedSuccessfully = false;
   		this.message = "";
        this.registration = {
          userName: "",
          password: "",
          confirmPassword: ""
        };
	}
  
  	signUp() {
      
      	let that=this;

        that.authService.saveRegistration(that.registration).then(function (response) {

            that.savedSuccessfully = true;
            that.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
            that.startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             that.message = "Failed to register user due to:" + errors.join(' ');
         });
    }
  
  	startTimer() {
      	let that=this;
        var timer = that.timeout(function () {
            that.timeout.cancel(timer);
           	that.location.path('root/login');
        }, 2000);
    }
     
}
signupCtrl.$inject=['$location', '$timeout', 'authService'];
export default signupCtrl;