
const SVC=new WeakMap();
const SCOPE=new WeakMap();
class BaseCtrl{      
    constructor(service, scope){            
        SVC.set(this, service);      
        SCOPE.set(this, scope);
        this.isNewItem= false;
        this.message = null;
        scope.list = null;
        this.pageNo= 1,
        this.pageSize=20,
        scope.gridOpts = { data: "list", pagingPageSize: this.pageSize, pagingPageSizes: [10, 15, 20, 30, 50, 100, 500, 1000], useExternalPaging: true };

        this.isGrid = false;

        scope.gridAction = {
            editAction: function (row) {
                this.model = this.onPreLoadForm(row.entity);
                this.isGrid = true;
                this.isNewItem = false;
            }.bind(this), removeAction: function (row) {
                if (confirm("Are you sure?")) {
                    this.removeItem(row.entity);
                }
            }.bind(this)
        };

        this.model = {};
        if( typeof toastr!=='undefined'){
            toastr.options.extendedTimeOut = 1000;
            toastr.options.timeOut = 1000;
            toastr.options.fadeOut = 250;
            toastr.options.fadeIn = 250;
            toastr.options.positionClass = "toast-top-center";
        }
        scope.$on('FilterValueChanged', function(event, obj){this.filterValueChanged(obj);}.bind(this));
    }
    filterValueChanged(obj){
      
    }
    createNewItem(){
        this.model = {};
        this.isGrid = true;
        this.isNewItem = true;
    }
    submitForm(){
       
        if (this.isNewItem) {
            this.insert(this.model);
        }
        else {
            this.update(this.model);
        }
    }
    getScope(){
        return SCOPE.get(this);
    }
    insert(data) {
        this.showSpinner();

        SVC.get(this).insert(data).success(function (res) {
               
            this.message = res.Message;

            this.showMessage(res.IsSuccess, res.Message);

            this.hideSpinner();

            this.isGrid = false;
            
            this.getPagedList();

        }.bind(this));


    }
   
    update(data) {
        this.showSpinner();

        SVC.get(this).update(data).success(function (res) {
          
            this.getPagedList();
               
            this.message = res.Message;

            this.showMessage(res.IsSuccess, res.Message);

            this.hideSpinner();

            this.isGrid = false;

        }.bind(this));

    }
    removeItem(data) {
        this.showSpinner();

        SVC.get(this).removeItem(data).success(function (res) {
            this.onAfterDeleted(data);

            this.message = res.Message;

            this.showMessage(res.IsSuccess, res.Message);

            this.hideSpinner();

        }.bind(this));

    }
    onAfterDeleted(item) {
       
    }
    getAll() {
        this.showSpinner();

        SVC.get(this).getAll().success(function (res) {
               
            this.list = res.DataList;

            this.showMessage(res.IsSuccess, res.Message);

            this.hideSpinner();

        }.bind(this));


    }
    getPagedList() {
       
        this.showSpinner();

        SVC.get(this).getPagedList(this.pageNo, this.pageSize).success(function (res) {
                
            SCOPE.get(this).gridOpts.totalItems = res.TotalRow;

            this.message = res.Message;

            if (res.DataList) {
                SCOPE.get(this).list = this.onPreLoadGrid(res.DataList);
            }
            this.showMessage(res.IsSuccess, res.Message);

            this.hideSpinner();

        }.bind(this));

    }
    parseDateTime(data) {
        var len = 0;
        if (data && (len = data.length) > 8) {
            return new Date(parseInt(data.substring(6, data.length - 2)));
        }
        return data;

    }
    onPreLoadGrid(dataList) {
        return dataList;
    }
    onPreLoadForm(item) {
        return item;
    }
    getPagedListWhile(data) {
        this.showSpinner();

        SVC.get(this).getPagedListWhile(this.pageNo, this.pageSize, data).success(function (res) {
               
            this.gridOpts.totalItems = res.TotalRow;

            this.message = res.Message;


            if (res.DataList) {
                this.list = this.onPreLoadGrid(res.DataList);

            }
            this.showMessage(res.IsSuccess, res.Message);

            this.hideSpinner();

        }.bind(this));


    }   
    success(message) {
        toastr["success"](message);
    }
    info(message) {
        toastr["info"](message);
    }
    warning(message) {
        toastr["warning"](message);
    }
    error(message) {
        toastr["error"](message);
    }
    showMessage(isSuccess, message) {
        if (isSuccess) {
            this.success(message);
        }
        else {
            this.warning(message);
        }
    }
    showSpinner() {
        jQuery(".jwt-spinner").show();
    }
    hideSpinner() {
        jQuery(".jwt-spinner").hide();
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
export default BaseCtrl;
