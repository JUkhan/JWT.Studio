using CSharp.Wrapper.JS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Wrapper.Angular;
using Scripts.Services;
namespace Scripts.Controllers
{
    public interface IBaseScope<T> : IScope
    {
        T model { get; set; }
        List<T> list { get; set; }
        string message { get; set; }
        GridOptions gridOpts { get; set; }
        bool isGrid { get; set; }
        GridActionExternalScope gridAction { get; set; }
    }
    public class BaseController<T>:jsClass
    {
        private bool isNewItem = false;
        private IBaseScope<T> scope = null;
        private BaseService<T> service = null;
        private Sce sce = null;
        protected int pageNo=1;
        protected int pageSize = 20;
        public BaseController(IBaseScope<T> scope, BaseService<T> service, Sce sce){
        
            this.scope = scope;
            this.service = service;
            this.sce = sce;
            this.scope.message = null;
            this.scope.list = null;
            this.scope.gridOpts = new GridOptions
            {
                 data="list",
                 pagingPageSize=this.pageSize,
                 pagingPageSizes = new List<int> { 10,15,20,30,50,100,500,1000},
                 useExternalPaging=true
            };
            this.scope.isGrid = false;
            this.scope.gridAction = new GridActionExternalScope
            {
                EditAction = row => { this.scope.model = row.entity; this.scope.isGrid = true; this.isNewItem = false; },
                RemoveAction = row => { if (confirm("Are you sure?")) {
                    this.Delete(row.entity); 
                    }
                }
            };
            /*$
                this.scope.model ={ };
                this.scope.TrustAsHtml=this.TrustAsHtml.bind(this);                
                this.scope.createNewItem=this.createNewItem.bind(this);
                this.scope.submitForm=this.submitForm.bind(this);
                toastr.options.extendedTimeOut = 1000;
                toastr.options.timeOut = 1000;
                toastr.options.fadeOut = 250;
                toastr.options.fadeIn = 250;
                toastr.options.positionClass = "toast-top-center";
             $*/
        }

        public virtual void createNewItem()
        {
           /*$  this.scope.model ={ };  $*/
          this.scope.isGrid = true;
          this.isNewItem = true;
      }
      public virtual void submitForm()
      {
          if (this.isNewItem)
          {
              this.Insert(this.scope.model);
          }
          else
          {
              this.Update(this.scope.model);
          }
      }
      public virtual void Insert(T data)
      {
          this.ShowSpinner();
          this.service.Insert(data).then(p =>
          {                
              JSONResult<T> res = p;                
              this.scope.message = res.Message;
              this.ShowMessage(res.IsSuccess, res.Message);
              this.HideSpinner();
              this.scope.isGrid = false;
              this.scope.list.Add(this.OnBeforeAddInList(res.DataObject));
          });
      }
      protected virtual T OnBeforeAddInList(T item) { return item; }
      public virtual void Update(T data)
      {
          this.ShowSpinner();
          this.service.Update(data).then(p =>
          {
              JSONResult<T> res = p;
              this.scope.message = res.Message;
              this.ShowMessage(res.IsSuccess, res.Message);
              this.HideSpinner();
              this.scope.isGrid = false;
          });
      }
      public virtual void Delete(T data)
      {
          this.ShowSpinner();
          this.service.Delete(data).then(p =>
          {
              this.OnAfterDeleted(data);
              JSONResult<T> res = p;
              this.scope.message = res.Message;
              this.ShowMessage(res.IsSuccess, res.Message);
              this.HideSpinner();
             
          });
      }
      protected virtual void OnAfterDeleted(T item) { }
      protected virtual void GetAll()
      {
          this.ShowSpinner();
          this.service.GetAll().then(p =>
          {
              JSONResult<T> res = p;                
              this.scope.list = res.DataList;
              this.ShowMessage(res.IsSuccess, res.Message);
              this.HideSpinner();
          });
      }
      protected virtual void GetPaged()
      {
          this.ShowSpinner();
          this.service.GetPaged(this.pageNo, this.pageSize).then(p =>
          {
              JSONResult<T> res = p;
              this.scope.gridOpts.totalItems= res.TotalRow;
              this.scope.message = res.Message;
              if (isValid(res.DataList))
              {
                  this.scope.list = this.OnPreLoad(res.DataList);
              }
              this.ShowMessage(res.IsSuccess, res.Message);
              this.HideSpinner();
          });
      }
      protected virtual dynamic ParseDateTime(dynamic data)
      {
          /*$  var len=0;
            if (data && (len= data.length) > 8)
            {
               return new Date(parseInt(data.substring(6, data.length - 2)));
            } $*/
          return data;
      }
      protected virtual List<T> OnPreLoad(List<T> dataList)
      {
          return dataList;
      }
      protected virtual void GetPagedWhile(T data)
      {
          this.ShowSpinner();
          this.service.GetPagedWhile(this.pageNo, this.pageSize,data).then(p =>
          {
              JSONResult<T> res = p;
              this.scope.gridOpts.totalItems = res.TotalRow;
              this.scope.message = res.Message;
              if (isValid(res.DataList))
              {
                  this.scope.list = this.OnPreLoad(res.DataList);
              }
              this.ShowMessage(res.IsSuccess, res.Message);
              this.HideSpinner();
          });
      }
      public virtual string TrustAsHtml(string html)
      {
          return this.sce.trustAsHtml(html);
      }
      public virtual void Success(string message)
      {       
          /*$           
          toastr["success"](message);
          $*/
        }
        public virtual void Info(string message)
        {            
            /*$           
            toastr["info"](message);           

            $*/
        }
        public virtual void Warning(string message)
        {           
            /*$            
            toastr["warning"](message);           

            $*/
        }
        public virtual void Error(string message)
        {            
            /*$            
            toastr["error"](message);           

            $*/
        }
        public virtual void ShowMessage(bool isSuccess, string message)
        {
            if (isSuccess) { this.Success(message); }
            else { this.Warning(message); }
        }
        public virtual void ShowSpinner()
        {

            jQuery(".jwt-spinner").show();
        }
        public virtual void HideSpinner()
        {

            jQuery(".jwt-spinner").hide();
        }

    }
}
