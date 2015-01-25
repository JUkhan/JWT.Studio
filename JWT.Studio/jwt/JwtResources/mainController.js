
var mainController = function ($scope, $http, $modal) {
    var scope = $scope;
    scope.msg = 'Main controller message...';
    scope.layoutList = [];
    scope.stateList = [];
    scope.parentList = [];
    scope.tplList = [];
    $http.get('Jwt/GetAllState').success(function (res) {
        scope.layoutList = res.layout;
        var temp = ['--Select--'];
        res.layout.ForEach(function (u) { temp.push(u.StateName); });
        scope.parentList = temp;
        scope.stateList = res.state;
        
    });
    $http.get('Jwt/GetTemplateList').success(function (res) { scope.tplList = res.data; });
    scope.getTemplateList = function () { $http.get('Jwt/GetTemplateList').success(function (res) { scope.tplList = res.data; }); };
    scope.layout = {};
    scope.state = { StateViews: [] };
    scope.saveLayout = function () {
        if (validate(scope.layout)) {
            if (scope.layout.Id) { update(scope.layout); return; }
            if (scope.layout.Parent === '--Select--') { scope.layout.Parent = ''; }
            scope.layout.IsAbstract = true;
            $http.post('Jwt/AddState', scope.layout)
           .success(function (res) {
               if (res.msg === 'Already Exist') { scope.msg = res.msg; }
               else {
                   scope.layout.id = res.msg;
                   scope.layoutList.push(scope.layout);
                   scope.parentList.push(scope.layout.StateName);
                   scope.layout = {};
               }
           });
        }
    };
    scope.layoutNameChanged = function (name) {
        scope.layout.Url = '/' + name;
    };
    scope.update = function (u) {
        scope.layout = u;
    };
    scope.remove = function (u) {
        if (confirm('Sure to remove ?')) {
            $http.post('Jwt/Remove', u)
            .success(function (res) {
                scope.msg = res.msg;
                scope.layoutList.remove(function (x) { return x.Id == u.Id; });
                scope.parentList.remove(function (x) { return x === u.StateName; });
            });
        }
    };
    //state///
    scope.saveSate = function () {
        if (validate(scope.state)) {
            if (scope.state.Id) { updateState(scope.state); return; }
            if (scope.state.Parent === '--Select--') { scope.state.Parent = ''; }
            scope.state.IsAbstract = false;            
            $http.post('Jwt/AddState', scope.state)
           .success(function (res) {
               if (res.msg === 'Already Exist') { scope.msg = res.state; }
               else {
                   scope.state.id = res.msg;
                   scope.stateList.push(scope.state);                   
                   scope.state = {StateViews:[]};
               }
           });
        }
    };
    scope.$watch('state.TemplateUrl', function (newVal, oldVal) {
       
        if (newVal) {
            newVal = newVal.replace('.html', '');
            scope.state.Url = '/' + newVal;
            scope.state.StateController = newVal + 'Ctrl';
            scope.state.StateName = newVal
        }
    });
    scope.updateState = function (u) {
        scope.state = u;
    };
    scope.removeState = function (u) {
        if (confirm('Sure to remove ?')) {
            $http.post('Jwt/Remove', u)
            .success(function (res) {
                scope.msg = res.msg;
                scope.stateList.remove(function (x) { return x.Id == u.Id; });                
            });
        }
    };
    /////
    scope.generateConfig = function () {
        $http.get('Jwt/GenerateConfig').success(function (res) { scope.msg = res.msg; alert(res.msg); });
    };
    function updateState(u) {       
        u.IsAbstract = false;
        $http.post('Jwt/Update', u)
       .success(function (res) {
           scope.msg = res.msg;
           scope.state = {};
       });
    }
    function update(u) {
        u.IsAbstract = true;
        $http.post('Jwt/Update', u)
       .success(function (res) {
           scope.msg = res.msg;
           scope.layout = {};
       });
    }
    function validate(u) {

        if (!u.StateName) {
            scope.msg = 'Name is required.';
            return false;
        }
        if (!u.Url) {
            scope.msg = 'Url is required.';
            return false;
        }
        if (u.StateViews && u.StateViews.length) {
            return true;
        }
        if (!u.TemplateUrl) {
            scope.msg = 'Template is required.';
            return false;
        }
        return true;
    }
    //////////////////Entities/////////////////////
    var list = [];

    scope.entities = [];
    scope.propList = [];
    scope.details = [];
    scope.selectedEntity = null;
    scope.selectedEntityDetails = '';
    scope.GetEntityList = function () {
        $http.get('Jwt/GetEntityList').success(function (res) {
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
        $http.get('Jwt/GetProperties?entityName=' + entity).success(function (res) {
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
        $http.post('Jwt/CodeGenerate', { entity: scope.selectedEntity, props: list[scope.selectedEntity] })
        .success(function (res) { alert(res.message); });
    };
    ////////////////////////////////////
    scope.showViewsDialog = function (layout) {
       
        $http.get('Jwt/GetViewList?stateName={0}&stateName2={1}'.format(layout.Parent, layout.StateName))
        .success(function (res) {
            if (res.success) {
                scope.state.StateViews = res.data;
                var modalInstance = $modal.open({
                    templateUrl: 'myModalContent.html',
                    controller: 'ModalInstanceCtrl',
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {views:scope.state.StateViews, tplList:scope.tplList};
                        }
                    }
                });
            }
        });
        
    }
    scope.$on('jwt-view-update', function (e, data) { scope.state.StateViews = data; });
};

function ModalInstanceCtrl($scope, $rootScope, $modalInstance, data) {
    $scope.items = data.views;
    $scope.tplList = data.tplList;
    $scope.ok = function () {        
        $rootScope.$broadcast('jwt-view-update', $scope.items);
        $modalInstance.dismiss('cancel');
    };
    $scope.templateNameChange= function (u) {        
        if (u.TemplateUrl) {
            u.ControllerName = u.TemplateUrl.replace('.html', 'Ctrl');
        }
    };
}
function root() {
    var path = window.location.pathname.toLowerCase().replace('/jwt','');   
    return path;
}