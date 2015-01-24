
(function(window){

var BaseController=jsClass.extend({
isNewItem:false,
scope:null,
service:null,
sce:null,
pageNo:1,
pageSize:20,
init:function(scope,service,sce)
{
this.scope=scope;

this.service=service;

this.sce=sce;

this.scope.message=null;

this.scope.list=null;

this.scope.gridOpts={data:"list",pagingPageSize:this.pageSize,pagingPageSizes:[10,15,20,30,50,100,500,1000],useExternalPaging:true};

this.scope.isGrid=false;

this.scope.gridAction={EditAction:function(row){this.scope.model=this.OnPreLoadForm(row.entity);

this.scope.isGrid=true;

this.isNewItem=false;

}.bind(this),RemoveAction:function(row){
if(confirm("Are you sure?")){this.Delete(row.entity);

}
}.bind(this)};

                this.scope.model ={ };                this.scope.TrustAsHtml=this.TrustAsHtml.bind(this);                                this.scope.createNewItem=this.createNewItem.bind(this);                this.scope.submitForm=this.submitForm.bind(this);                toastr.options.extendedTimeOut = 1000;                toastr.options.timeOut = 1000;                toastr.options.fadeOut = 250;                toastr.options.fadeIn = 250;                toastr.options.positionClass = "toast-top-center";             

},
createNewItem:function()
{
  this.scope.model ={ };  this.scope.isGrid=true;

this.isNewItem=true;


},
submitForm:function()
{

if(this.isNewItem){this.Insert(this.scope.model);

}

else{this.Update(this.scope.model);

}

},
Insert:function(data)
{
this.ShowSpinner();

this.service.Insert(data).then(function(p){var res=p;
this.scope.message=res.Message;

this.ShowMessage(res.IsSuccess,res.Message);

this.HideSpinner();

this.scope.isGrid=false;

this.scope.list.Add(this.OnBeforeAddInList(res.DataObject));

}.bind(this));


},
OnBeforeAddInList:function(item)
{

return item;


},
Update:function(data)
{
this.ShowSpinner();

this.service.Update(data).then(function(p){var res=p;
this.scope.message=res.Message;

this.ShowMessage(res.IsSuccess,res.Message);

this.HideSpinner();

this.scope.isGrid=false;

}.bind(this));


},
Delete:function(data)
{
this.ShowSpinner();

this.service.Delete(data).then(function(p){this.OnAfterDeleted(data);

var res=p;
this.scope.message=res.Message;

this.ShowMessage(res.IsSuccess,res.Message);

this.HideSpinner();

}.bind(this));


},
OnAfterDeleted:function(item)
{

},
GetAll:function()
{
this.ShowSpinner();

this.service.GetAll().then(function(p){var res=p;
this.scope.list=res.DataList;

this.ShowMessage(res.IsSuccess,res.Message);

this.HideSpinner();

}.bind(this));


},
GetPaged:function()
{
this.ShowSpinner();

this.service.GetPaged(this.pageNo,this.pageSize).then(function(p){var res=p;
this.scope.gridOpts.totalItems=res.TotalRow;

this.scope.message=res.Message;


if(isValid(res.DataList)){this.scope.list=this.OnPreLoadGrid(res.DataList);

}
this.ShowMessage(res.IsSuccess,res.Message);

this.HideSpinner();

}.bind(this));


},
ParseDateTime:function(data)
{
  var len=0;            if (data && (len= data.length) > 8)            {               return new Date(parseInt(data.substring(6, data.length - 2)));            } 
return data;


},
OnPreLoadGrid:function(dataList)
{

return dataList;


},
OnPreLoadForm:function(item)
{

return item;


},
GetPagedWhile:function(data)
{
this.ShowSpinner();

this.service.GetPagedWhile(this.pageNo,this.pageSize,data).then(function(p){var res=p;
this.scope.gridOpts.totalItems=res.TotalRow;

this.scope.message=res.Message;


if(isValid(res.DataList)){this.scope.list=this.OnPreLoadGrid(res.DataList);

}
this.ShowMessage(res.IsSuccess,res.Message);

this.HideSpinner();

}.bind(this));


},
TrustAsHtml:function(html)
{

return this.sce.trustAsHtml(html);


},
Success:function(message)
{
                     toastr["success"](message);          
},
Info:function(message)
{
                       toastr["info"](message);                       
},
Warning:function(message)
{
                        toastr["warning"](message);                       
},
Error:function(message)
{
                        toastr["error"](message);                       
},
ShowMessage:function(isSuccess,message)
{

if(isSuccess){this.Success(message);

}

else{this.Warning(message);

}

},
ShowSpinner:function()
{
jQuery(".jwt-spinner").show();


},
HideSpinner:function()
{
jQuery(".jwt-spinner").hide();


}});
namespace('Scripts.Controllers.BaseController',BaseController);

})(window);