using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
namespace jwtTest.AppScript.Services
{
    public class BaseService<T>:jsClass
    {
        public string controllerName=null;
        private QService qService = null;
        private HttpService http = null;
        public BaseService(string controllerName,HttpService http, QService qService)
        {
            this.controllerName = controllerName;
            this.http = http;
            this.qService = qService;
        }
        public virtual Promise Insert(T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("/{0}/Insert".format(this.controllerName), data)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise InsertEntities(List<T> dataList)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("/{0}/InsertEntities".format(this.controllerName), dataList)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise Update(T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("/{0}/Update".format(this.controllerName), data)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise UpdateEntities(List<T> dataList)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("/{0}/UpdateEntities".format(this.controllerName), dataList)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetAll()
        {
            Deferred deffer = this.qService.defer();
            this.http.get("/{0}/GetAll".format(this.controllerName))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetPaged(int pageNo, int pageSize)
        {
            Deferred deffer = this.qService.defer();
            this.http.get("/{0}/GetPaged?pageNo={1}&pageSize={2}".format(this.controllerName, pageNo, pageSize))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetPagedWhile(int pageNo, int pageSize, T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("/{0}/GetPagedWhile".format(this.controllerName), new { pageNo = pageNo, pageSize = pageSize, item = data })
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetByID(Object id)
        {
            Deferred deffer = this.qService.defer();
            this.http.get("/{0}/GetByID?ID={1}".format(this.controllerName, id))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise Count()
        {
            Deferred deffer = this.qService.defer();
            this.http.get("/{0}/Count".format(this.controllerName))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise Delete(T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("/{0}/Delete".format(this.controllerName), data)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
    }
}
