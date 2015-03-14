
const HTTP=new WeakMap();
class BaseSvc
{
    constructor(http){
        HTTP.set(this,http);
    }
    getDummyData(obj){     
        return new Promise((resolve, reject)=>{
             HTTP.get(this).post('Jwt/GetDummyData',obj)
        		.success(res=>{ resolve(angular.fromJson(res.data)); });
        }); 
    }
}
export default BaseSvc;