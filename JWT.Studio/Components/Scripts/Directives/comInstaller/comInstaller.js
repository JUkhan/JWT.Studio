
class comInstaller{
    constructor(){       
        this.restrict='E';
        this.templateUrl='Scripts/Directives/comInstaller/comInstaller.html';
        
        this.scope={
            name:"@",
            description:'@'
        };
       this.controller=function($scope){
            $scope.installComponent=function(name){
                sendMessage($scope.name);
            }
        };
       

      
    }
   
   
    static builder(){
        comInstaller.instance=new comInstaller();
        return  comInstaller.instance;
    }

}
//comInstaller.builder.$inject =['$rootScope'];
export default comInstaller;
