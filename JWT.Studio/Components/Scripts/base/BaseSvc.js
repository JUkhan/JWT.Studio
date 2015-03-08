
const HTTP=new WeakMap();
class BaseSvc{       
    constructor(controllerName, http) {
        this.controllerName = controllerName;
        HTTP.set(this,http);
    }
    root() {
        return "";
    }
    insert(data) {
        return HTTP.get(this).post(this.root()+this.controllerName+'/Insert',data);          
    }
    insertEntities(dataList) {
        return HTTP.get(this).post(this.root()+this.controllerName+'/InsertEntities',dataList);    
        
    }
    update(data) {
        return HTTP.get(this).post(this.root()+this.controllerName+'/Update',data);            
    }
    updateEntities (dataList) {
        return HTTP.get(this).post(this.root()+this.controllerName+'/UpdateEntities',dataList);             

    }
    getAll() {
        return HTTP.get(this).get(this.root()+this.controllerName+'/GetAll');  
            
    }
    getPagedList(pageNo, pageSize) {
        return HTTP.get(this).get(this.root()+this.controllerName+'/GetPaged?pageNo='+pageNo+'&pageSize='+pageSize);  
         
    }
    getPagedListWhile(pageNo, pageSize, data) {
        return HTTP.get(this).get(this.root()+this.controllerName+'/GetPagedWhile?pageNo='+pageNo+'&pageSize='+pageSize+'&item='+data); 
        
    }
    getByID(id) {
        return HTTP.get(this).get(this.root()+this.controllerName+'/GetByID?ID='+id);            

    }
    count() {
        return HTTP.get(this).get(this.root()+this.controllerName+'/Count');            
    }
    removeItem(data) {
        return HTTP.get(this).post(this.root()+this.controllerName+'/Delete', data); 
    }
}

export default BaseSvc;