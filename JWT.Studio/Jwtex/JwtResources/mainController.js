
angular.module('jwt2').controller('mainController', ['$scope', '$http', '$modal', '$q', function (scope, http, modal, qService) {
    
    toastr.options.extendedTimeOut = 1000;
    toastr.options.timeOut = 1000;
    toastr.options.fadeOut = 250;
    toastr.options.fadeIn = 250;
    toastr.options.positionClass = "toast-top-right";
   
    scope.jsModel = '';
    scope.htmlModel = '';
    scope.jsList = [];
    scope.jsFileName = '';
    scope.htmlList = [];
    scope.htmlFileName = '';
    scope.jsEditor = null;
    scope.htmlEditor = null;
    setJsEditor(scope);
    setHtmlEditor(scope);
    //tab_javascript
    scope.jsSearch = function (val) {
        if (val !== scope.jsModel) {
            http.get('JwtEx/GetFileList/?directory=' + val).success(function (data) {
                if (data.isSuccess) { scope.jsList = data.data; if (scope.jsEditor) { scope.jsEditor.setValue(''); } scope.jsFileName = ''; } else { info(data.msg); }
            });
        }
        scope.jsModel = val;
    };    
    scope.jsLoad = function (fileName) {
        scope.jsFileName = fileName;
        if (!fileName) { info('{0} file name is required.'.format(scope.jsModel)); return; }
        http.get('JwtEx/GetFileContent/?fileName={0}&key={1}'.format(fileName, scope.jsModel))
           .success(function (data) {              
               scope.jsEditor.setValue(data.data);
           });

    }
    scope.saveJsFile = function () {
        if (!scope.jsFileName) { info('There is no file to be saved.'); return; }
        http.post('JwtEx/SaveFile', { key: scope.jsModel, fileName: scope.jsFileName, content: scope.jsEditor.getValue() })
        .success(function (data) { if (data.isSuccess) { success('Saved successfully.'); } });
    }
    //tab_html
    scope.htmlSearch = function (val) {
        if (val !== scope.htmlModel) {
            http.get('JwtEx/GetFileList/?directory=' + val).success(function (data) {
                if (data.isSuccess) { scope.htmlList = data.data; if (scope.htmlEditor) { scope.htmlEditor.setValue(''); }scope.htmlFileName = null; } else { info(data.msg); }
            });
        }
        scope.htmlModel = val;
    };   
    scope.htmlLoad = function (fileName) {
        scope.htmlFileName = fileName;
        if (!fileName) { info('{0} file name is required.'.format(scope.htmlModel)); return; }
        http.get('JwtEx/GetFileContent/?fileName={0}&key={1}'.format(fileName, scope.htmlModel))
           .success(function (data) {
               scope.htmlEditor.setValue(data.data);
           });

    }
    scope.saveHtmlFile = function () {
        if (!scope.htmlFileName) { info('There is no file to be saved.'); return; }
        http.post('JwtEx/SaveFile', { key: scope.htmlModel, fileName: scope.htmlFileName, content: scope.htmlEditor.getValue() })
        .success(function (data) { if (data.isSuccess) { success('Saved successfully.'); } });
    }
    scope.jsSearch('Controllers');
    scope.htmlSearch('Layouts');
    info('Welcome to jwt.');
    function success(msg) {
        toastr['success'](msg);
    }
    function info(msg) {
        toastr['info'](msg);
    }
}]);


function setJsEditor(scope) {
    setTimeout(function () {
     scope.jsEditor= CodeMirror.fromTextArea(document.getElementById("jsEditor"), {
            lineNumbers: true,
            theme: 'rubyblue',
            lineWrapping: true,
            mode:{name: 'javascript',globalVars: true},
            matchBrackets: true,
            autoCloseBrackets: true,
            extraKeys: { "Ctrl-Space": "autocomplete" },           
            enableSearchTools: true,
            styleActiveLine: true
        });
    }, 100);
}
function setHtmlEditor(scope) {
    setTimeout(function () {
        scope.htmlEditor = CodeMirror.fromTextArea(document.getElementById("htmlEditor"), {
            lineNumbers: true,
            theme: 'rubyblue',
            lineWrapping: true,
            mode: 'htmlmixed',
            matchBrackets: true,
            matchTags: { bothTags: true },
            autoCloseBrackets: true,
            autoCloseTags: true,
            enableSearchTools: true,
            styleActiveLine: true
        });
    }, 200);
}