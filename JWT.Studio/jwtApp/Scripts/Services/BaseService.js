
(function (window) {

    var BaseService = jsClass.extend({
        controllerName: null,
        qService: null,
        http: null,
        init: function (controllerName, http, qService) {
            this.controllerName = controllerName;

            this.http = http;

            this.qService = qService;



        },
        Root: function () {

            return "";


        },
        Insert: function (data) {
            var deffer = this.qService.defer();
            this.http.post("{0}/{1}/Insert".format(this.Root(), this.controllerName), data).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        InsertEntities: function (dataList) {
            var deffer = this.qService.defer();
            this.http.post("{0}/{1}/InsertEntities".format(this.Root(), this.controllerName), dataList).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        Update: function (data) {
            var deffer = this.qService.defer();
            this.http.post("{0}/{1}/Update".format(this.Root(), this.controllerName), data).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        UpdateEntities: function (dataList) {
            var deffer = this.qService.defer();
            this.http.post("{0}/{1}/UpdateEntities".format(this.Root(), this.controllerName), dataList).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        GetAll: function () {
            var deffer = this.qService.defer();
            this.http.get("{0}/{1}/GetAll".format(this.Root(), this.controllerName)).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        GetPaged: function (pageNo, pageSize) {
            var deffer = this.qService.defer();
            this.http.get("{0}/{1}/GetPaged?pageNo={2}&pageSize={3}".format(this.Root(), this.controllerName, pageNo, pageSize)).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        GetPagedWhile: function (pageNo, pageSize, data) {
            var deffer = this.qService.defer();
            this.http.post("{0}/{1}/GetPagedWhile".format(this.Root(), this.controllerName), { pageNo: pageNo, pageSize: pageSize, item: data }).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        GetByID: function (id) {
            var deffer = this.qService.defer();
            this.http.get("{0}/{1}/GetByID?ID={2}".format(this.Root(), this.controllerName, id)).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        Count: function () {
            var deffer = this.qService.defer();
            this.http.get("{0}/{1}/Count".format(this.Root(), this.controllerName)).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        },
        Delete: function (data) {
            var deffer = this.qService.defer();
            this.http.post("{0}/{1}/Delete".format(this.Root(), this.controllerName), data).success(function (res, status) {
                deffer.resolve(res);

            }.bind(this)).error(function (res, status) {
                deffer.reject(res);

            }.bind(this));


            return deffer.promise;


        }
    });
    namespace('Scripts.Services.BaseService', BaseService);

})(window);