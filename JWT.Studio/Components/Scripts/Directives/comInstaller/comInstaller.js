
class comInstaller{
    constructor(){       
        this.restrict='E';
        this.templateUrl='Scripts/Directives/comInstaller/comInstaller.html';
        
        this.scope={
            name:"@",
            description:'@'
        };
        this.controller=function($scope, $http, $modal){
           $scope.installComponent=function(name){
               sendMessage($scope.name);
           };
           $scope.getDemoInfo=function(name, mode){
               $http.get('JwtComponent/GetDemoInfo?name={0}&mode={1}'.format(name, mode))
               .success(function(res){                   
                   var info=res.data, title=mode==='api'?'Api':'Demo Code';
                   var modalInstance = $modal.open({
                       templateUrl: 'myModalContent.html',
                       controller: function($scope, $modalInstance, data){
                           $scope.info=data.info;
                           $scope.title=data.title;
                           $scope.ok=function(){
                               $modalInstance.dismiss('cancel');
                           };
                          
                       },
                       size: 'lg',
                       resolve: {
                           data: function () {
                               return { info:info , title:title};
                           }
                       }
                   });
               });
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
