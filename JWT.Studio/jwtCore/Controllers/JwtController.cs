using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Jwt.Dao.Service;
namespace Jwt.Core.Controllers
{
    public class JwtController<T, ID> : Controller
    {
        IBaseService<T> service;
        public void SetService(IBaseService<T> service)
        {
            this.service = service;
        }
       
        public virtual JsonResult Insert(T data)
        {
            JSONResult json = new JSONResult();
            try
            {
                data = service.Insert(data);
                json.DataObject = data;
                json.IsSuccess = true;
                json.Message = "Successfull";
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json);
        }
       
        public virtual JsonResult InsertEntities(List<T> dataList)
        {
            JSONResult json = new JSONResult();
            try
            {
                dynamic data = service.InsertEntities(dataList);
                json.DataList = data;
                json.IsSuccess = true;
                json.Message = "Successfull";
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json);
        }
     
        public virtual JsonResult Update(T data)
        {
            JSONResult json = new JSONResult();
            try
            {
                service.Update(data);
                json.IsSuccess = true;
                json.Message = "Successfull";

            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json);
        }
      
        public virtual JsonResult UpdateEntities(List<T> dataList)
        {
            JSONResult json = new JSONResult();
            try
            {
                service.UpdateEntities(dataList);
                json.IsSuccess = true;
                json.Message = "Successfull";
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json);
        }
       
        public virtual JsonResult GetByID(ID Id)
        {
            JSONResult json = new JSONResult();
            try
            {
                T data = service.GetByID(Id);
                json.DataObject = data;
                json.IsSuccess = true;
                if (data == null)
                    json.Message = "Item not found.";
                else
                    json.Message = "Successfull";
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
       
        public virtual JsonResult Count()
        {
            JSONResult json = new JSONResult();
            try
            {
                json.TotalRow = service.Count();
                json.Message = "Successfull";
                json.IsSuccess = true;
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
     
        public virtual JsonResult GetAll()
        {
            JSONResult json = new JSONResult();
            try
            {
                dynamic dataList = service.GetAll();
                json.DataList = dataList;
                json.Message = "Successfull";
                json.IsSuccess = true;
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
      
        public virtual JsonResult GetPaged(int pageNo, int pageSize)
        {
            JSONResult json = new JSONResult();
            try
            {

                PagedList dataList = service.GetPaged(pageNo, pageSize);
                json.DataList = dataList.Data;
                json.TotalRow = dataList.Total;
                json.Message = "Data loaded.";
                json.IsSuccess = true;
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }       
        public virtual JSONResult GetPagedWhile(int pageNo, int pageSize, T item)
        {
            JSONResult json = new JSONResult();
            try
            {
                PagedList res = service.GetPagedWhile(pageNo, pageSize, item);
                json.DataList = res.Data;
                json.TotalRow = res.Total;
                json.Message = "Data loaded.";
                json.IsSuccess = true;
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return json;
        }
       
        public virtual JsonResult Delete(T data)
        {
            JSONResult json = new JSONResult();
            try
            {
                data = service.Delete(data);
                json.DataObject = data;
                json.IsSuccess = true;
                json.Message = "Successfull";

            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.ToString();
            }
            return Json(json);
        }
    }
}
