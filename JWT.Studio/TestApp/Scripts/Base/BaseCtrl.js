
class BaseCtrl{      
    constructor(scope){        
        scope.$on('FilterValueChanged', function(event, obj){this.filterValueChanged(obj);}.bind(this));
    }
    filterValueChanged(obj){
     
    } 
    initFilter() {
        var scope=this;
        if (window.sessionStorage["jwtFilter"]) {
            var ob = angular.fromJson(window.sessionStorage["jwtFilter"]);
            if (angular.isObject(ob)) {
                for (var prop in ob) {					
                    if (scope.hasOwnProperty(prop)) {
                        scope[prop] = ob[prop];
                    }
                }
            }
        }
    }
    gRunner(g){
        let it=g(),ret;
        (function iterate(val){
            ret=it.next(val);
            if(!ret.done){             
                if(ret.value){
                    if('success' in ret.value){
                        ret.value.success(iterate);
                    }
                    else if('then' in ret.value){
                        ret.value.then(iterate);
                    }
                    else{
                        iterate(ret.value);
                    }
                }
            }
        })();
    }
}
export default BaseCtrl;
