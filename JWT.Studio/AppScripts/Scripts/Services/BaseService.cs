using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharp.Wrapper.Angular;
using CSharp.Wrapper.JS;
namespace Scripts.Services
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
        public virtual string Root()
        {
           return "";
        }
        public virtual Promise Insert(T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("{0}/{1}/Insert".format(this.Root(), this.controllerName), data)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise InsertEntities(List<T> dataList)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("{0}/{1}/InsertEntities".format(this.Root(), this.controllerName), dataList)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise Update(T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("{0}/{1}/Update".format(this.Root(), this.controllerName), data)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise UpdateEntities(List<T> dataList)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("{0}/{1}/UpdateEntities".format(this.Root(), this.controllerName), dataList)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetAll()
        {
            Deferred deffer = this.qService.defer();
            this.http.get("{0}/{1}/GetAll".format(this.Root(), this.controllerName))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetPaged(int pageNo, int pageSize)
        {
            Deferred deffer = this.qService.defer();
            this.http.get("{0}/{1}/GetPaged?pageNo={2}&pageSize={3}".format(this.Root(), this.controllerName, pageNo, pageSize))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetPagedWhile(int pageNo, int pageSize, T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("{0}/{1}/GetPagedWhile".format(this.Root(), this.controllerName), new { pageNo = pageNo, pageSize = pageSize, item = data })
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise GetByID(Object id)
        {
            Deferred deffer = this.qService.defer();
            this.http.get("{0}/{1}/GetByID?ID={2}".format(this.Root(), this.controllerName, id))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise Count()
        {
            Deferred deffer = this.qService.defer();
            this.http.get("{0}/{1}/Count".format(this.Root(), this.controllerName))
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
        public virtual Promise Delete(T data)
        {
            Deferred deffer = this.qService.defer();
            this.http.post("{0}/{1}/Delete".format(this.Root(), this.controllerName), data)
                .success((res, status) => { deffer.resolve(res); })
                .error((res, status) => { deffer.reject(res); });
            return deffer.promise;
        }
    }
}
