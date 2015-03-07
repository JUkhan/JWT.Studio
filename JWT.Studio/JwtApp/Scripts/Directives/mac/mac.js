
class mac{
  constructor(rootScope){ 
    this.restrict='E';   
    this.templateUrl='Scripts/Directives/mac/mac.html'; 
  }   
  static builder(){ 
    return  new mac();
  }
}

export default mac;
