

class mac{
    constructor(rootScope){
        console.log('mac initialized succ...');
        this.restrict='E';
        this.templateUrl='Scripts/Directives/mac/mac.html';
    }
   
   
    static builder(){
       
        return  new mac();
    }

}

export default mac;
