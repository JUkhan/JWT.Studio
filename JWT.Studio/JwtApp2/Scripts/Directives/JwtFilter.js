
const ROOTSCOPE=new WeakMap();
class JwtFilter{
    constructor(rootScope){
        console.log('gwt-filter initialized succ...');
        this.restrict='A';
        ROOTSCOPE.set(this,rootScope);
    }
    setVal(prop, val) {
        var ob = window.sessionStorage.getItem("jwtFilter") || "{}",
        data = angular.fromJson(ob);
        data[prop] = val;
        window.sessionStorage.setItem("jwtFilter", angular.toJson(data));
    }
    link(scope, jquery, attrs, ctrl){
        var filterNaame = attrs.dbFilter || attrs.ngModel;
        scope.$watch(filterNaame, function (newVal, oldVal) {
            filterNaame=filterNaame.replace('vm.','');
            ROOTSCOPE.get(JwtFilter.instance).$broadcast("FilterValueChanged", { name: filterNaame, newValue: newVal, oldValue: oldVal });
            JwtFilter.instance.setVal(filterNaame, newVal);
        });
    }
    static builder(rootScope){
        JwtFilter.instance=new JwtFilter(rootScope);
        return  JwtFilter.instance;
    }

}
JwtFilter.builder.$inject =['$rootScope'];
export default JwtFilter;
