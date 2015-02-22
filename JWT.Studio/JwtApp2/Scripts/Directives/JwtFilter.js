
const ROOTSCOPE=new WeakMap();
class JwtFilter{
    constructor(rootScope){
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
            ROOTSCOPE.get(JwtFilter.instance).$broadcast("FilterValueChanged", { name: filterNaame, newValue: newVal, oldValue: oldVal });
            JwtFilter.instance.setVal(filterNaame, newVal);
        });
    }
    static builder(rootScope){
        JwtFilter.instance=new JwtNotifier(rootScope);
        return  JwtFilter.instance;
    }

}
JwtFilter.builder.$instance=['$rootScope'];
export default JwtFilter;
