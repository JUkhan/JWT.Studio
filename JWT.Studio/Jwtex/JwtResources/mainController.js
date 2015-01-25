
angular.module('jwt2').controller('mainController', ['$scope', '$http', '$modal', '$q', function (scope, http, modal, qService) {
    
    scope.msg = 'Main controller message...';
   
    scope.jsModel = '';
    scope.htmlModel = '';
    scope.jsList = [];
    scope.jsFileName = '';
    scope.htmlList = [];
    scope.htmlFileName = '';
    setJsEditor(scope);
    setHtmlEditor(scope);
    //tab_javascript
    scope.jsSearch = function (val) {
        if (val !== scope.jsModel) {
            http.get('JwtEx/GetFileList/?directory=' + val).success(function (data) {
                if (data.isSuccess) { scope.jsList = data.data; console.log(data); } else { alert(data.msg); }
            });
        }
        scope.jsModel = val;
    };    
    scope.jsLoad = function (fileName) {
        scope.jsFileName = fileName;
        http.get('JwtEx/GetFileContent/?fileName={0}&key={1}'.format(fileName, scope.jsModel))
           .success(function (data) {
               scope.jsEditor.setValue(data.data);
           });

    }
    scope.saveJsFile = function () {

        http.post('JwtEx/SaveFile', { key: scope.jsModel, fileName: scope.jsFileName, content: scope.jsEditor.getValue() })
        .success(function (data) { console.log(data); });
    }
    //tab_html
    scope.htmlSearch = function (val) {
        if (val !== scope.htmlModel) {
            http.get('JwtEx/GetFileList/?directory=' + val).success(function (data) {
                if (data.isSuccess) { scope.htmlList = data.data; } else { alert(data.msg); }
            });
        }
        scope.htmlModel = val;
    };   
    scope.htmlLoad = function (fileName) {
        scope.htmlFileName = fileName;
        http.get('JwtEx/GetFileContent/?fileName={0}&key={1}'.format(fileName, scope.htmlModel))
           .success(function (data) {
               scope.htmlEditor.setValue(data.data);
           });

    }
    scope.saveHtmlFile = function () {

        http.post('JwtEx/SaveFile', { key: scope.htmlModel, fileName: scope.htmlFileName, content: scope.htmlEditor.getValue() })
        .success(function (data) { console.log(data); });
    }
    scope.jsSearch('Controllers');
    scope.htmlSearch('Layouts');
}]);


function setJsEditor(scope) {
    setTimeout(function () {
     scope.jsEditor= CodeMirror.fromTextArea(document.getElementById("jsEditor"), {
            lineNumbers: true,
            theme: 'rubyblue',
            lineWrapping: true,
            mode: 'javascript',
            matchBrackets: true,
            autoCloseBrackets: true,
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