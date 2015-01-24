
(function (window) {

    var CourseCtrl = Scripts.Controllers.BaseController.extend({
        scope: null,
        service: null,
        init: function (scope, service, sce) {
            this._super(scope, service, sce);
            this.scope = scope;

            this.service = service;

            scope.gridOpts.columnDefs = [{ name: "AC", width: 50, enableSorting: false, cellTemplate: "<div style='text-align:center'><a ng-click=\"getExternalScopes().EditAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-pencil\"></i>  </a><a ng-click=\"getExternalScopes().RemoveAction(row)\" href=\"javascript:;\"> <i class=\"fa fa-trash\"></i>  </a></div>" }, { name: "Title" }, { name: "Credits" }];

            scope.gridOpts.onRegisterApi = function (gridApi) {
                gridApi.paging.on.pagingChanged(scope, function (newPage, pageSize) {
                    this.pageNo = newPage;

                    this.pageSize = pageSize;

                    this.GetPaged();

                }.bind(this));

            }.bind(this);

            this.GetPaged();

            this.LoadRelationalData();



        },
        OnAfterDeleted: function (item) {
            this.scope.list.remove(function (x) { return x.CourseID == item.CourseID; }.bind(this));


        },
        LoadRelationalData: function () {

        }
    });
    namespace('Scripts.Controllers.CourseCtrl', CourseCtrl);

    angular.module('app').controller('CourseCtrl', ['$scope', 'CourseService', '$sce', CourseCtrl]);
})(window);