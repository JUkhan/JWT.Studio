
class loginCtrl 
{
	constructor(scope, location, authService, ngAuthSettings){
      	this.scope=scope;
		this.authService=authService;
      	this.location=location;
      	this.ngAuthSettings=ngAuthSettings;
		this.init();
	}
  
  	init(){
         this.loginData = {
          userName: "",
          password: "",
          useRefreshTokens: false
      	};

    	this.message = "";
    }
  
  	login(){
      let that=this;
      this.authService.login(this.loginData)
         .then(function (response) {that.location.path('root/home');},
               function (err) { that.message = err.error_description; });
      }
  
  	authExternalProvider(provider){
      
      	var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';
		console.log(redirectUri);
        var externalProviderUrl = this.ngAuthSettings.stsServiceBaseUri 
        + "api/Account/ExternalLogin?provider=" + provider
        + "&response_type=token&client_id=" + this.ngAuthSettings.clientId + "&redirect_uri=" + redirectUri;
        window.$windowScope = this;

        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", 
                                      "location=0,status=0,width=600,height=750");
      
    }
  
  	authCompletedCB (fragment) {
      	let that=this;

        that.scope.$apply(function () {

            if (fragment.haslocalaccount == 'False') {

                this.authService.logOut();

                that.authService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };

                that.location.path('associate');

            }
            else {
                //Obtain access token and redirect to orders
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                that.authService.obtainAccessToken(externalData).then(function (response) {

                    that.location.path('root/home');

                },
             function (err) {
                that.message = err.error_description;
             });
            }

        });
    }
}
loginCtrl.$inject=['$scope','$location', 'authService', 'ngAuthSettings'];
export default loginCtrl;