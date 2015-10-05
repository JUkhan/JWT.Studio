
app.controller('mainController', ['$scope', '$http', '$modal', function (scope, http, modal) {
    
    //layouts
    scope.layoutList = [];
    scope.lvm = {Abstract:false};
    scope.addLayout = function (vm) {
        if (!vm.LayoutName) {
            return;
        }
        var url = 'Jwt/';
        url += (typeof (vm._id) === 'undefined') ? 'AddLayout' : 'UpdateLayout';
        overlay(1);
        http.post(url, vm).success(function (res) {
            getLayouts();
            scope.lvm = { Abstract: false };
            scope.generateConfig();
            overlay(0);
        });
    }
    scope.updateLayout = function (vm) {
        scope.lvm = vm;
    }
    scope.removeLayout = function (vm) {
        if (!confirm('Sure to remove?')) { return; }
        overlay(1);
        http.post('Jwt/RemoveLayout', vm).success(function (res) {
            getLayouts(); scope.lvm = { Abstract: false };
            scope.generateConfig();
            overlay(0);
        });
    }
    function getLayouts() {
        http.get('Jwt/GetLayoutList').success(function (res) { scope.layoutList = res; });
    }
    getLayouts();
    //navigations
    scope.navList = [];
    scope.nvm = {};
    scope.addNav = function (vm) {
        if (!vm.NavigationName) {
            return;
        }
        var url = 'Jwt/';
        url += (typeof (vm._id) === 'undefined') ? 'AddNavigation' : 'UpdateNavigation';
        overlay(1);
        http.post(url, vm).success(function (res) {
            getNavs();
            scope.nvm = {};
            scope.generateConfig();
            overlay(0);
        });
    }
    scope.updateNav = function (vm) {
        scope.nvm = vm;
    }
    scope.removeNav = function (vm) {
        if (!confirm('Sure to remove?')) { return; }
        overlay(1);
        http.post('Jwt/RemoveNavigation', vm).success(function (res) {
            getNavs(); scope.nvm = {};
            scope.generateConfig();
            overlay(0);
        });
    }
    function getNavs() {
        http.get('Jwt/GetNavigationList').success(function (res) { scope.navList = res; });
    }
    scope.getTemplateList=function() {
        http.get('Jwt/GetTemplateList').success(function (res) { scope.widgetList = res.data; });
    }
    getNavs(); scope.getTemplateList();

    scope.showViewsDialog = function (nav) {

        http.get('Jwt/GetViewList?layoutName={0}&navName={1}'.format(nav.HasLayout, nav.NavigationName))
        .success(function (res) {
            if (res.success) {
                scope.nvm.UIViews = res.data;
                var modalInstance = modal.open({
                    templateUrl: 'myModalContent.html',
                    controller: 'ModalInstanceCtrl',
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return { views: scope.nvm.UIViews, tplList: scope.widgetList };
                        }
                    }
                });
            }
        });

    }
    scope.$on('jwt-view-update', function (e, data) { scope.nvm.UIViews = data; });
    scope.generateConfig = function () {
        http.get('Jwt/GenerateConfig').success(function (res) { scope.msg = res.msg;  });
    };
    //second tab
    var list = [];

    scope.entities = [];
    scope.propList = [];
    scope.details = [];
    scope.selectedEntity = null;
    scope.selectedEntityDetails = '';
    scope.GetEntityList = function () {
        http.get('Jwt/GetEntityList').success(function (res) {
            if (res.success) {
                scope.entities = res.data;
            } else {
                alert(res.message);
            }
        });

    };
    scope.entityDetails = function (entity) {
        scope.details = [];
        scope.selectedEntityDetails = '';
        scope.selectedEntity = entity;
        if (list[entity]) { scope.propList = list[entity]; return; }
        http.get('Jwt/GetProperties?entityName=' + entity).success(function (res) {
            if (res.success) {
                list[entity] = res.data; scope.propList = res.data;
            } else {
                alert(res.message);
            }
        });
    };
    scope.showDetails = function (u) {
        scope.selectedEntityDetails = u.PropertyName;
        scope.details = u.Details;
    };

    scope.CodeGenerate = function () {
        if (!scope.selectedEntity) { return }
        http.post('Jwt/CodeGenerate', { entity: scope.selectedEntity, props: list[scope.selectedEntity] })
        .success(function (res) { alert(res.message); });
    };
   
}]);


function ModalInstanceCtrl($scope, $rootScope, $modalInstance, data) {
    $scope.items = data.views;
    $scope.tplList = data.tplList;
    $scope.ok = function () {
        $rootScope.$broadcast('jwt-view-update', $scope.items);
        $modalInstance.dismiss('cancel');
    };
    $scope.templateNameChange = function (u) {
        if (u.TemplateUrl) {
            u.ControllerName = u.TemplateUrl.replace('.html', 'Ctrl');
        }
    };
}
function root() {
    var path = window.location.pathname.toLowerCase().replace('/jwt', '');
    return path;
}
function overlay(val) {
    val ? $('.overlay').show() : $('.overlay').hide();
}
app.controller('ModalInstanceCtrl', ModalInstanceCtrl);