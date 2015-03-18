
const HTTP=new WeakMap();
class BaseSvc
{
    constructor(http){
        HTTP.set(this,http);
    }
    getDummyData(obj){ 
      
           return  HTTP.get(this).post('Jwt/GetDummyData',obj);        		
    }
}
export default BaseSvc;