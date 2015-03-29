
angular.module('jwt2').controller('mainController', ['$scope', '$http', '$modal', '$q', 'jwtSvc', '$timeout', function (scope, http, modal, qService, jwtSvc, $timeout) {

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
            unlocakAll();
            scope.items = [];
            scope.jsList = [];
            scope.htmlList = [];
            scope.cssList = [];
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
            unlocakAll();
            scope.jsList = res.js;
            scope.htmlList = res.html;
            scope.cssList = res.css;
            setEditorDefault();
            if (res.js && res.js.length > 0) {
                scope.jsFileName = res.js[0];
                scope.jsLoad(scope.jsFileName);
            }
            if (res.html && res.html.length > 0)
                scope.htmlFileName = res.html[0];
            if (res.css && res.css.length > 0)
                scope.cssFileName = res.css[0];

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
    function unlocakAll() {
        
        if (scope.dataChange.jsf) {
            saveFile(scope.dataMode, scope.jsFileName.replace('.js', ''), scope.jsFileName, scope.jsEditor.getValue());           
            scope.dataChange.jsf = false;
        }
        if (scope.dataChange.htmlf) {           
            saveFile(scope.dataMode, scope.htmlFileName.replace('.html', ''), scope.htmlFileName, scope.htmlEditor.getValue());           
            scope.dataChange.htmlf = false;
        }
        if (scope.dataChange.cssf) {
            saveFile(scope.dataMode, scope.cssFileName.replace('.css', ''), scope.cssFileName, scope.cssEditor.getValue());          
            scope.dataChange.cssf = false;
        }
    }
    function saveFile(mode, directoryName, fileName, content) {
        http.post('JwtEx/SaveFile', { mode: mode, directoryName: directoryName, fileName: fileName, content: content })
       .success(function (data) {
           if (data.isSuccess) {             
               success('Saved successfully.');
               jwtSvc.unlock({ Category: mode, Folder: directoryName, Name: fileName });
           }
       });
    }
    //end of new style

    //tab_javascript

    scope.jsLoad = function (fileName) {
        scope.jsFileName = fileName;
        if (!fileName) { info('File name is required.'); return; }
        http.get('JwtEx/GetFileContent/?mode={0}&directoryName={1}&fileName={2}'.format(scope.dataMode, scope.items[scope.itemValue], fileName))
           .success(function (data) {
               AWAIT.jsLock = 0;
               scope.jsEditor.setValue(data.data);
               $timeout(function () { AWAIT.jsLock = 1; LOCK.jsLock = data.locked; }, 100);
               updateEditor(scope.jsFileName, data.locked, true);
           });

    }
    scope.saveJsFile = function () {
        if (!scope.jsFileName) { info('There is no file to be saved.'); return; }
        if (LOCK.jsLock) return;
        http.post('JwtEx/SaveFile', { mode: scope.dataMode, directoryName: scope.items[scope.itemValue], fileName: scope.jsFileName, content: scope.jsEditor.getValue() })
        .success(function (data) {
            if (data.isSuccess) {
                jwtSvc.unlock({ Category: scope.dataMode, Folder: scope.items[scope.itemValue], Name: scope.jsFileName });
                success('Saved successfully.');
                AWAIT.jsLock = 1;
                scope.dataChange.jsf = false;
            }
        });
    }
    //tab_html

    scope.htmlLoad = function (fileName) {
        scope.htmlFileName = fileName;
        if (!fileName) { info('File name is required.'); return; }
        http.get('JwtEx/GetFileContent/?mode={0}&directoryName={1}&fileName={2}'.format(scope.dataMode, scope.items[scope.itemValue], fileName))
           .success(function (data) {
               AWAIT.htmlLock = 0;
               scope.htmlEditor.setValue(data.data);
               $timeout(function () { AWAIT.htmlLock = 1; LOCK.htmlLock = data.locked; }, 100);
               updateEditor(scope.htmlFileName, data.locked, true);
           });

    }
    scope.saveHtmlFile = function () {
        if (!scope.htmlFileName) { info('There is no file to be saved.'); return; }
        if (LOCK.htmlLock) return;
        http.post('JwtEx/SaveFile', { mode: scope.dataMode, directoryName: scope.items[scope.itemValue], fileName: scope.htmlFileName, content: scope.htmlEditor.getValue() })
        .success(function (data) {
            if (data.isSuccess) {
                jwtSvc.unlock({ Category: scope.dataMode, Folder: scope.items[scope.itemValue], Name: scope.htmlFileName });
                success('Saved successfully.');
                AWAIT.htmlLock = 1;
                scope.dataChange.htmlf = false;
            }
        });
    }

    //tab_css

    scope.cssLoad = function (fileName) {
        scope.cssFileName = fileName;
        if (!fileName) { info('File name is required.'); return; }
        http.get('JwtEx/GetFileContent/?mode={0}&directoryName={1}&fileName={2}'.format(scope.dataMode, scope.items[scope.itemValue], fileName))
           .success(function (data) {
               AWAIT.cssLock = 0;
               scope.cssEditor.setValue(data.data);
               $timeout(function () { AWAIT.cssLock = 1; LOCK.cssLock = data.locked; }, 100);
               updateEditor(scope.cssFileName, data.locked, true);
           });

    }
    scope.saveCssFile = function () {
        if (!scope.cssFileName) { info('There is no file to be saved.'); return; }
        if (LOCK.cssLock) return;
        http.post('JwtEx/SaveFile', { mode: scope.dataMode, directoryName: scope.items[scope.itemValue], fileName: scope.cssFileName, content: scope.cssEditor.getValue() })
        .success(function (data) {
            if (data.isSuccess) {
                jwtSvc.unlock({ Category: scope.dataMode, Folder: scope.items[scope.itemValue], Name: scope.cssFileName });
                success('Saved successfully.');
                AWAIT.cssLock = 1;
                scope.dataChange.cssf = false;
            }
        });
    }
    //signalR
    var LOCK = { jsLock: 0, htmlLock: 0, cssLock: 0 };
    var AWAIT = { jsLock: 0, htmlLock: 0, cssLock: 0 };
    scope.lock = LOCK;
    scope.dataChange = { jsf: 0, htmlf: 0, cssf: 0 };
    scope.users = [];
    scope.user = "me";
    scope.$on('newConnection', function (event, user) {      
        info(user + ' Joined in Development');
        scope.users.push(user);
        scope.$apply();
    });
    scope.$on('removeConnection', function (event, user) {
        info(user + ' Disconnected from Development');
        scope.users.remove(function (user2) { return user2 === user; });
        scope.$apply();
    });
    scope.$on('onlineUsers', function (event, users) {
        scope.users = users;

        scope.$apply();
    });
    scope.$on('lockFile', function (event, file) {
        var folder = scope.items[scope.itemValue];
        if (scope.dataMode === file.Category) {
            scope.user = file.UserName;
            if (file.Category === 'Base') {
                updateEditor(file.Name, true, false);
            }
            else if (file.Folder === folder) {
                updateEditor(file.Name, true, false);
            }           
        }
    });
    scope.$on('unlockFile', function (event, file) {
        var folder = scope.items[scope.itemValue];
        if (scope.dataMode === file.Category) {
            if (file.Category === 'Base') {
                updateEditor(file.Name, false, false);
            }
            else if (file.Folder === folder) {
                updateEditor(file.Name, false, false);
            }
        }
    });
    scope.jsChange = function () {
        if (AWAIT.jsLock) {
            if (scope.jsEditor.getValue()) {
                try{  jwtSvc.lock({ Category: scope.dataMode, Folder: scope.items[scope.itemValue] || 'base', Name: scope.jsFileName });}catch(error){}
                AWAIT.jsLock = 0;
                scope.$apply(function () { scope.dataChange.jsf = true; });

            }
        }
    };
    scope.htmlChange = function () {
        if (AWAIT.htmlLock) {
            if (scope.htmlEditor.getValue()) {
                try{ jwtSvc.lock({ Category: scope.dataMode, Folder: scope.items[scope.itemValue], Name: scope.htmlFileName });}catch(error){}
                AWAIT.htmlLock = 0;
                scope.$apply(function () { scope.dataChange.htmlf = true; });
            }
        }
    };
    scope.cssChange = function () {
        if (AWAIT.cssLock) {
            if (scope.cssEditor.getValue()) {
                try{jwtSvc.lock({ Category: scope.dataMode, Folder: scope.items[scope.itemValue], Name: scope.cssFileName });}catch(error){}
                AWAIT.cssLock = 0;
                scope.$apply(function () { scope.dataChange.cssf = true; });
            }
        }
    };
    function updateEditor(fileName, readOnly, isFromLoadContent) {

        if (fileName === scope.jsFileName) {
            scope.jsEditor.options.readOnly = readOnly;
            if (!isFromLoadContent)
                scope.$apply(function () { LOCK.jsLock = readOnly; });
            if (!LOCK.jsLock && !isFromLoadContent) { scope.jsLoad(fileName); info('Unlocked ' + fileName); }
        }
        else if (fileName === scope.htmlFileName) {
            scope.htmlEditor.options.readOnly = readOnly;
            if (!isFromLoadContent)
                scope.$apply(function () { LOCK.htmlLock = readOnly; });
            if (!LOCK.htmlLock && !isFromLoadContent) { scope.htmlLoad(fileName); info('Unlocked ' + fileName); }
        }
        else if (fileName === scope.cssFileName) {
            scope.cssEditor.options.readOnly = readOnly;
            if (!isFromLoadContent)
                scope.$apply(function () { LOCK.cssLock = readOnly; });
            if (!LOCK.cssLock && !isFromLoadContent) { scope.cssLoad(fileName); info('Unlocked ' + fileName); }
        }

    }
    //end signalR
    //online user
    var modalInstance = null, chatUser='';
    scope.messageList = [];
    scope.$on('receiveMessage', function (event, data) {
        scope.messageList[data.sender] = scope.messageList[data.sender] || [];
        scope.messageList[data.sender].push(data);
        scope.$apply();
        if (chatUser !== data.sender) {
            if (modalInstance)
            modalInstance.close();
            openPopup(scope.user, data.sender, scope.messageList[data.sender]);
            chatUser = data.sender;
        } else {           
                modalInstance.close();
                openPopup(scope.user, data.sender, scope.messageList[data.sender]);          
        }
        
        scrollTop();
    });
    scope.toggle = false;
    scope.onclick = function () {
        scope.toggle = !scope.toggle;
    };
    scope.showPopup = function (user) {
        chatUser = user;
        scope.messageList[user] = scope.messageList[user] || [];
        openPopup(scope.user, user, scope.messageList[user]);
    };
    //lg,sm
    function openPopup(sender, sendto, list) {
        modalInstance = modal.open({
            templateUrl: 'chatModal.html',
            controller: 'ModalInstanceCtrl',
            backdrop: 'static',
            size: 'lg',
            resolve: {
                data: function () {
                    return { sender: sender, sendto: sendto, list: list };
                }
            }
        });
       
    }
    //end online user
    function success(msg) {
        toastr['success'](msg);
    }
    function info(msg) {
        toastr['info'](msg);
    }
}]);

function scrollTop() {
    var messageArea = $('.messageArea');
    messageArea.scrollTop(messageArea.get(0).scrollHeight);
}
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
        scope.jsEditor.on("change", scope.jsChange);
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
            styleActiveLine: true,
            extraKeys: { "Ctrl-Space": "autocomplete", "Ctrl-S": function (ins) { scope.saveHtmlFile(); } },
        });
        scope.htmlEditor.on("change", scope.htmlChange);
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
        scope.cssEditor.on("change", scope.cssChange);
    }, 300);
}
function overlay(val) {
    val ? $('.overlay').show() : $('.overlay').hide();
}