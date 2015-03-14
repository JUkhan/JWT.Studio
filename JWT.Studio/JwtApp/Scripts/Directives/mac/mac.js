class mac{ 
  
  constructor(){
    this.restrict='EA'; 
    
    this.templateUrl='Scripts/Directives/mac/mac.html';
  }     
  static builder(){ 
    return  new mac();  
  }
}
export default mac;