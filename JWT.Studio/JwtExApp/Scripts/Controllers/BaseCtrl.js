
export default class BaseCtrl{

    constructor(scope)
    {
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
                    let temp=prop.replace('vm.','');
                    if (scope.hasOwnProperty(temp)) {
                        scope[temp] = ob[prop];
                    }
                }
            }
        }
    }
    async(g){
        let it=g(),ret;
        (function iterate(val){
            ret=it.next(val);
            if(!ret.done){             
                if(ret.value){
                    if('success' in ret.value){
                        ret.value.success(iterate);
                    }
                    else{
                        iterate(ret.value);
                    }
                }
            }
        })();
    }
} 