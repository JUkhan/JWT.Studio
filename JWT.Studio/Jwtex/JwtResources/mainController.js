﻿
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
    scope.cssList = [];
    scope.jsEditor = null;
    scope.htmlEditor = null;
    setJsEditor(scope);
    setHtmlEditor(scope);
    setCssEditor(scope);

    //new style
    scope.dataMode = '';
    scope.itemValue = '';
    scope.items = [];
    scope.changeDataMode = function (val) {
        if (val !== scope.dataMode) {
            if (val === 'Base') {                
                scope.dataMode = val;
                scope.changeItemValue();
                return;
            }
            http.get('JwtEx/GetItems/?name=' + val)
            .success(function (res) {
                scope.items = res;
                scope.itemValue = '0';
                setEditorDefault();

            })
        }
        scope.dataMode = val;
    }
    scope.changeItemValue = function () {
        if (scope.itemValue === '0') { return; }
        http.get('JwtEx/GetItemDetail/?name={0}&mode={1}'.format(scope.items[scope.itemValue], scope.dataMode))
        .success(function (res) {
            scope.jsList = res.js;
            scope.htmlList = res.html;
            scope.cssList = res.css;
            setEditorDefault();
        });
    };
    scope.newWidget = function (name) {
        if (!name) return;
        createItem(name, 'Widgets');

    };
    scope.newComponent = function (name) {
        if (!name) return
        createItem(name, 'Components');
    };
    function createItem(name, mode) {
        overlay(1);
        http.get('JwtEx/IsExist/?name={0}&mode={1}'.format(name, mode))
        .success(function (res) {
            if (!res.exist) {
                http.get('JwtEx/CreateItem/?name={0}&mode={1}'.format(name, mode))
                .success(function (res) {
                    if (res.success) {
                        success('Successfully generated.');
                    }
                    else {
                        info(res.msg);
                    }
                    overlay(0);
                });
            }
            else {
                info('Already exist.');
                overlay(0);
            }
        });
    }
    function setEditorDefault() {
        scope.htmlEditor.setValue('');
        scope.jsEditor.setValue('');
        scope.cssEditor.setValue('');
        scope.jsFileName = '';
        scope.htmlFileName = '';
        scope.cssFileName = '';
    }
    //end of new style

    //tab_javascript

    scope.jsLoad = function (fileName) {
        scope.jsFileName = fileName;
        if (!fileName) { info('File name is required.'); return; }
        http.get('JwtEx/GetFileContent/?mode={0}&directoryName={1}&fileName={2}'.format(scope.dataMode, scope.items[scope.itemValue], fileName))
           .success(function (data) {
               scope.jsEditor.setValue(data.data);
           });

    }
    scope.saveJsFile = function () {
        if (!scope.jsFileName) { info('There is no file to be saved.'); return; }
        http.post('JwtEx/SaveFile', { mode: scope.dataMode, directoryName: scope.items[scope.itemValue], fileName: scope.jsFileName, content: scope.jsEditor.getValue() })
        .success(function (data) { if (data.isSuccess) { success('Saved successfully.'); } });
    }
    //tab_html

    scope.htmlLoad = function (fileName) {
        scope.htmlFileName = fileName;
        if (!fileName) { info('File name is required.'); return; }
        http.get('JwtEx/GetFileContent/?mode={0}&directoryName={1}&fileName={2}'.format(scope.dataMode, scope.items[scope.itemValue], fileName))
           .success(function (data) {
               scope.htmlEditor.setValue(data.data);
           });

    }
    scope.saveHtmlFile = function () {
        if (!scope.htmlFileName) { info('There is no file to be saved.'); return; }
        http.post('JwtEx/SaveFile', { mode: scope.dataMode, directoryName: scope.items[scope.itemValue], fileName: scope.htmlFileName, content: scope.htmlEditor.getValue() })
        .success(function (data) { if (data.isSuccess) { success('Saved successfully.'); } });
    }

    //tab_css

    scope.cssLoad = function (fileName) {
        scope.cssFileName = fileName;
        if (!fileName) { info('File name is required.'); return; }
        http.get('JwtEx/GetFileContent/?mode={0}&directoryName={1}&fileName={2}'.format(scope.dataMode, scope.items[scope.itemValue], fileName))
           .success(function (data) {
               scope.cssEditor.setValue(data.data);
           });

    }
    scope.saveCssFile = function () {
        if (!scope.cssFileName) { info('There is no file to be saved.'); return; }
        http.post('JwtEx/SaveFile', { mode: scope.dataMode, directoryName: scope.items[scope.itemValue], fileName: scope.cssFileName, content: scope.cssEditor.getValue() })
        .success(function (data) { if (data.isSuccess) { success('Saved successfully.'); } });
    }
    //scope.jsSearch('Controllers');
    //scope.htmlSearch('Layouts');
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
        scope.jsEditor = CodeMirror.fromTextArea(document.getElementById("jsEditor"), {
            lineNumbers: true,
            theme: 'rubyblue',
            lineWrapping: true,
            mode: { name: 'javascript', globalVars: true },
            matchBrackets: true,
            autoCloseBrackets: true,
            extraKeys: { "Ctrl-Space": "autocomplete", "Ctrl-S": function (ins) { scope.saveJsFile(); } },
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
            mode: 'text/html',
            matchBrackets: true,
            matchTags: { bothTags: true },
            autoCloseBrackets: true,
            autoCloseTags: true,
            enableSearchTools: true,
            styleActiveLine: true,
            extraKeys: { "Ctrl-Space": "autocomplete", "Ctrl-S": function (ins) { scope.saveHtmlFile(); } },
        });
    }, 200);
}
function setCssEditor(scope) {
    setTimeout(function () {
        scope.cssEditor = CodeMirror.fromTextArea(document.getElementById("cssEditor"), {
            lineNumbers: true,
            theme: 'rubyblue',
            lineWrapping: true,
            mode: 'text/css',
            matchBrackets: true,
            matchTags: { bothTags: true },
            autoCloseBrackets: true,
            autoCloseTags: true,
            enableSearchTools: true,
            styleActiveLine: true,
            extraKeys: { "Ctrl-Space": "autocomplete", "Ctrl-S": function (ins) { scope.saveCssFile(); } },
        });
    }, 300);
}
function overlay(val) {
    val ? $('.overlay').show() : $('.overlay').hide();
}